using System;
using System.Collections.Generic;

#nullable disable

namespace Shop.DAL.Models
{
    public partial class OrderGood
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GoodsId { get; set; }
        public byte Count { get; set; }

        public virtual Good Goods { get; set; }
        public virtual Order Order { get; set; }
    }
}
