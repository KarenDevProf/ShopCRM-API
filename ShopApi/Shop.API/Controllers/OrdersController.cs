using Microsoft.AspNetCore.Mvc;
using Shop.BusinessLayer.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Shop.API.Models;
using Shop.BusinessLayer.Models.Orders.RequestModels;
using Shop.BusinessLayer.Models.Orders.ResponseModels;
using System;

namespace Shop.API.Controllers
{
    public class OrdersController : ShopBaseController
    {
        private readonly IOrders _orders;

        public OrdersController(IShopServices services) : base(services)
        {
            _orders = services.GetService<IOrders>();
        }

        [HttpGet]
        [Route("orders")]
        public async Task<ResponseObjectModel<List<OrderResponse>>> GetOrders()
        {
            ResponseObjectModel<List<OrderResponse>> responseObject = new ResponseObjectModel<List<OrderResponse>>();
            var allOrders = await _orders.GetOrdersAsync();
            responseObject.Data = Mapper.Map<List<OrderResponse>>(allOrders);
            return responseObject;
        }

        [HttpGet]
        [Route("orders/{ordernum}")]
        public async Task<ResponseObjectModel<OrderResponse>> GetOrderByOrderNum(short ordernum)
        {
            ResponseObjectModel<OrderResponse> responseObject = new ResponseObjectModel<OrderResponse>();
            var order = await _orders.GetOrderByOrderNumAsync(ordernum);
            responseObject.Data = Mapper.Map<OrderResponse>(order);
            return responseObject;
        }

        [HttpPost]
        [Route("orders/{ordernum}")]
        public async Task<ResponseObjectModel<OrderResponse>> CreateOrder(short ordernum, [FromBody] CreateOrderRequest createOrderRequest)
        {
            ResponseObjectModel<OrderResponse> responseObject = new ResponseObjectModel<OrderResponse>();

            var createdOrder = await _orders.CreateOrderAsync(ordernum, createOrderRequest);

            responseObject.Data = Mapper.Map<OrderResponse>(createdOrder);
            return responseObject;
        }

        [HttpPut]
        [Route("orders/{ordernum}")]
        public async Task<ResponseObjectModel<OrderResponse>> UpdateOrderPut(short ordernum, [FromBody] UpdateOrderRequest orderRequest)
        {
            ResponseObjectModel<OrderResponse> responseObject = new ResponseObjectModel<OrderResponse>();

            var updatedOrder = await _orders.UpdateOrderPutAsync(ordernum, orderRequest);

            responseObject.Data = Mapper.Map<OrderResponse>(updatedOrder);
            return responseObject;
        }

        [HttpPatch]
        [Route("orders/{ordernum}")]
        public async Task<ResponseObjectModel<OrderResponse>> UpdateOrderPatch(short ordernum, [FromBody] JsonPatchDocument orderRequest)
        {
            ResponseObjectModel<OrderResponse> responseObject = new ResponseObjectModel<OrderResponse>();

            var updatedOrder = await _orders.UpdateOrderPatchAsync(ordernum, orderRequest);

            responseObject.Data = Mapper.Map<OrderResponse>(updatedOrder);
            return responseObject;
        }

        [HttpDelete]
        [Route("orders/{ordernum}")]
        public async Task<ResponseObjectModel<object>> DeleteOrder(short ordernum)
        {
            ResponseObjectModel<object> responseObject = new ResponseObjectModel<object>();

            await _orders.DeleteOrderAsync(ordernum);
            return responseObject;
        }

        [HttpGet]
        [Route("orders/forDate")]
        public async Task<ResponseObjectModel<List<OrderResponse>>> GetOrderByOrderNum(DateTime date)
        {
            ResponseObjectModel<List<OrderResponse>> responseObject = new ResponseObjectModel<List<OrderResponse>>();
            var order = await _orders.GetOrderByDateAsync(date);
            responseObject.Data = Mapper.Map<List<OrderResponse>>(order);
            return responseObject;
        }
    }
}