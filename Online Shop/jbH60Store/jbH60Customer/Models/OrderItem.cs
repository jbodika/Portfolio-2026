using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace jbH60Customer.Models
{
    public  class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ValidateNever]
        public virtual Order Order { get; set; } = null!;

        [ValidateNever]
        public virtual Product Product { get; set; } = null!;
    }
}
