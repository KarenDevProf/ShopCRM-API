using System;
using System.Collections.Generic;

#nullable disable

namespace Shop.DAL.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
