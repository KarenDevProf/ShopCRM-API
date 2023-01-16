using System;
using System.Collections.Generic;

namespace Shop.BusinessLayer.Models.Orders.RequestModels
{
    public class UpdateOrderRequest
    {
        public short OrderNumber { get; set; }
        public int UserId { get; set; }
        public byte Status { get; set; }
        public List<OrderGoodsModel> OrderedGoods { get; set; }
    }
}
