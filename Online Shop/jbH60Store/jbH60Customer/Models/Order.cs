using System;
using System.Collections.Generic;

namespace jbH60Customer.Models
{
    public  class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateFufilled { get; set; }
        public decimal Total { get; set; }
        public decimal Taxes { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
