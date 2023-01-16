using System.Collections.Generic;

namespace Shop.BusinessLayer.Models.Orders.ResponseModels
{
    public class OrderResponse
    {
        public short OrderNumber { get; set; }
        public string UserFullName { get; set; }
        public string Status { get; set; }
        public List<OrderedGoods> OrderedGoods { get; set; }
    }
}
