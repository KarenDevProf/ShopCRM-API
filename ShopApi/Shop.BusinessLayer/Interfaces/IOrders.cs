using Microsoft.AspNetCore.JsonPatch;
using Shop.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shop.BusinessLayer.Models.Orders.RequestModels;
using System;

namespace Shop.BusinessLayer.Interfaces
{
    public interface IOrders
    {
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByOrderNumAsync(short orderNum);
        Task<Order> UpdateOrderPatchAsync(short orderNum, JsonPatchDocument orderDocument);
        Task<Order> CreateOrderAsync(short ordernum, CreateOrderRequest createOrderRequest);
        Task<Order> UpdateOrderPutAsync(short ordernum, UpdateOrderRequest orderRequest);
        Task DeleteOrderAsync(short ordernum);
        Task<List<Order>> GetOrderByDateAsync(DateTime date);
    }
}