using System;

namespace Shop.BusinessLayer.Models.Orders.RequestModels
{
    public class OrderGoodsModel
    {
        public int GoodsId { get; set; }
        public byte Count { get; set; }
    }
}
