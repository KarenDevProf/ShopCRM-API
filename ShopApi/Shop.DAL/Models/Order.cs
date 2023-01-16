using System;
using System.Collections.Generic;

#nullable disable

namespace Shop.DAL.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderGoods = new HashSet<OrderGood>();
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public short OrderNumber { get; set; }
        public byte Status { get; set; }
        public DateTime OrderCreateDate { get; set; }
        public DateTime OrderUpdateDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderGood> OrderGoods { get; set; }
    }
}
