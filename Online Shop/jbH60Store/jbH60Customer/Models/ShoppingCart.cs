using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace jbH60Customer.Models
{
    public class ShoppingCart
    {

        public int CartId { get; set; }

        public int CustomerId { get; set; }

        public DateTime DateCreated { get; set; }
      
        [ValidateNever]

        public virtual Customer Customer { get; set; }
        
        [ValidateNever]
        public ICollection<CartItem> CartItems { get; set; }
        public ShoppingCart() { }
    }
}
