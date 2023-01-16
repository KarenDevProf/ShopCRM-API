using System;
using System.Collections.Generic;

#nullable disable

namespace Shop.DAL.Models
{
    public partial class Good
    {
        public Good()
        {
            OrderGoods = new HashSet<OrderGood>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte Article { get; set; }
        public decimal Price { get; set; }

        public virtual ICollection<OrderGood> OrderGoods { get; set; }
    }
}
