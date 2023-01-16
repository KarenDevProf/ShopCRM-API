using System;
using System.Collections.Generic;
using Shop.BusinessLayer.Enums;

namespace Shop.BusinessLayer.Models.Orders.RequestModels
{
    public class CreateOrderRequest
    {
        public int UserId { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Registered;
        public List<OrderGoodsModel> OrderedGoods { get; set; }

    }
}
