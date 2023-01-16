using Microsoft.EntityFrameworkCore;
using Shop.BusinessLayer.Interfaces;
using Shop.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.Repositories.Repositories.Interfaces;
using Shop.BusinessLayer.Enums;
using Microsoft.AspNetCore.JsonPatch;
using Shop.BusinessLayer.Exceptions;
using Shop.BusinessLayer.Models.Orders.RequestModels;
using System.Linq;
using System;

namespace Shop.BusinessLayer.Services
{
    public class OrdersBl : IOrders
    {
        private readonly IRepository<Order> _order;
        private readonly IRepository<Good> _goods;
        private readonly IRepository<User> _users;
        private readonly IRepository<OrderGood> _orderGoods;
        public OrdersBl(IRepository<Order> order, IRepository<Good> goods, IRepository<OrderGood> orderGoods, IRepository<User> users)
        {
            _order = order;
            _orderGoods = orderGoods;
            _goods = goods;
            _users = users;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _order.FindBy(e => e.Status == (byte)OrderStatus.Registered)
                .Include(e => e.User)
                .Include(e => e.OrderGoods)
                .ThenInclude(x => x.Goods)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByOrderNumAsync(short orderNum)
        {
            var order = await _order.FindBy(e => e.OrderNumber == orderNum)
                .Include(e => e.User)
                .Include(e => e.OrderGoods)
                .ThenInclude(x => x.Goods)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            return order;
        }

        public async Task<Order> CreateOrderAsync(short ordernum, CreateOrderRequest createOrderRequest)
        {
            var isFreeOrderNumber = await IsFreeOrderNumber(ordernum);
            if (!isFreeOrderNumber)
            {
                throw new OrderNumberUsedException();
            }

            var goodsCount = createOrderRequest.OrderedGoods.Sum(e => e.Count);

            if (goodsCount > 10)
            {
                throw new OrderItemsCountException();
            }

            var goodsIds = createOrderRequest.OrderedGoods.ToList();
            var allOrderGoods = await _orderGoods.GetAll()
                .Include(e => e.Goods)
                .ThenInclude(e => e.OrderGoods)
                .ToListAsync();

            var data = goodsIds.Join(allOrderGoods, e => e.GoodsId, x => x.GoodsId, (e, b) => new
            {
                OrderGoodsModel = e,
                OrderGoods = b,
            }).Select(e => new { Count = e.OrderGoodsModel.Count, Price = e.OrderGoods.Goods.Price }).Distinct().ToList();

            var sumAmount = data.Sum(e => e.Count * e.Price);

            if (sumAmount > 15000)
            {
                throw new OrderAmountExceededException();
            }

            var order = new Order
            {
                OrderNumber = ordernum,
                Status = (byte)createOrderRequest.Status,
                UserId = createOrderRequest.UserId,
                User = await _users.FindBy(e => e.Id == createOrderRequest.UserId).FirstOrDefaultAsync(),
                OrderGoods = createOrderRequest.
                OrderedGoods.Select(e => new OrderGood
                {
                    GoodsId = e.GoodsId,
                    Count = e.Count
                }).ToList(),
                OrderCreateDate = DateTime.Now,
                OrderUpdateDate = DateTime.Now
            };

            await _order.AddAsync(order);
            await _order.SaveChangesAsync();

            return order;
        }

        public async Task<Order> UpdateOrderPutAsync(short ordernum, UpdateOrderRequest orderRequest)
        {
            var existingOrder = await _order
                .FindBy(e => e.OrderNumber == ordernum && e.Status == (byte)OrderStatus.Registered)
                .Include(e => e.User)
                .Include(e => e.OrderGoods)
                .ThenInclude(e => e.Goods)
                .FirstOrDefaultAsync();

            if (existingOrder == null)
            {
                throw new NotFoundException("Order");
            }

            if (ordernum != orderRequest.OrderNumber)
            {
                var isFreeOrderNumber = await IsFreeOrderNumber(orderRequest.OrderNumber);
                if (!isFreeOrderNumber)
                {
                    throw new OrderNumberUsedException();
                }
            }

            var goodsCount = orderRequest.OrderedGoods.Sum(e => e.Count);

            if (goodsCount > 10)
            {
                throw new OrderItemsCountException();
            }

            var goodsIds = orderRequest.OrderedGoods.ToList();
            var allOrderGoods = await _orderGoods.GetAll()
                .Include(e => e.Goods)
                .ThenInclude(e => e.OrderGoods)
                .ToListAsync();

            var data = goodsIds.Join(allOrderGoods, e => e.GoodsId, x => x.GoodsId, (e, b) => new
            {
                OrderGoodsModel = e,
                OrderGoods = b,
            }).Select(e => new { Count = e.OrderGoodsModel.Count, Price = e.OrderGoods.Goods.Price }).Distinct().ToList();

            var sumAmount = data.Sum(e => e.Count * e.Price);

            if (sumAmount > 15000)
            {
                throw new OrderAmountExceededException();
            }

            // Update existingOrder
            _order.SetCurrentValues(existingOrder, orderRequest);

            // Delete OrderGoods
            foreach (var existingOrderGoods in existingOrder.OrderGoods.ToList())
            {
                if (!orderRequest.OrderedGoods.Any(c => c.GoodsId == existingOrderGoods.GoodsId))
                    _orderGoods.Delete(existingOrderGoods);
            }

            // Update and Insert OrderGoods
            foreach (var og in orderRequest.OrderedGoods)
            {
                var existingOrderGoods = existingOrder.OrderGoods
                    .FirstOrDefault(c => c.GoodsId == og.GoodsId && c.GoodsId != default);

                if (existingOrderGoods != null)
                    // Update OrderGoods
                    _orderGoods.SetCurrentValues(existingOrderGoods, og);

                else
                {
                    // Insert OrderGoods
                    var newOrderGood = new OrderGood
                    {
                        GoodsId = og.GoodsId,
                        Count = og.Count,
                        Goods = await _goods.FindBy(e => e.Id == og.GoodsId).FirstOrDefaultAsync()
                    };
                    existingOrder.OrderGoods.Add(newOrderGood);
                }
            }

            existingOrder.OrderUpdateDate = DateTime.Now;

            await _order.SaveChangesAsync();

            return existingOrder;

        }

        public async Task<Order> UpdateOrderPatchAsync(short orderNum, JsonPatchDocument orderDocument)
        {
            Order order = await _order
                .FindBy(e => e.OrderNumber == orderNum && e.Status == (byte)OrderStatus.Registered)
                .Include(e => e.User)
                .Include(e => e.OrderGoods)
                .ThenInclude(e => e.Goods)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            orderDocument.ApplyTo(order);
            order.OrderUpdateDate = DateTime.Now;

            await _order.SaveChangesAsync();

            return order;
        }

        public async Task DeleteOrderAsync(short ordernum)
        {
            var order = await _order
                .FindBy(e => e.OrderNumber == ordernum && e.Status == (byte)OrderStatus.Registered)
                .FirstOrDefaultAsync();
            if (order == null)
            {
                throw new NotFoundException("Order");
            }

            _order.Delete(order);

            await _order.SaveChangesAsync();
        }

        public async Task<List<Order>> GetOrderByDateAsync(DateTime date)
        {
            var orders = await _order.FindBy(e => e.OrderCreateDate.Date == date.Date)
                .Include(e => e.User)
                .Include(e => e.OrderGoods)
                .ThenInclude(e => e.Goods).ToListAsync();

            if (orders == null)
            {
                throw new NotFoundException("Order");
            }

            return orders;
        }

        private async Task<bool> IsFreeOrderNumber(short orderNumber)
        {
            return await _order.FindBy(e => e.OrderNumber == orderNumber).FirstOrDefaultAsync() == null;
        }

    }
}
