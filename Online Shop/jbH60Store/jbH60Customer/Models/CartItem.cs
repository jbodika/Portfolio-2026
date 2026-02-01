using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace jbH60Customer.Models
{
    public class CartItem
    {

        public int CartItemId { get; set; }
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
     
        [ValidateNever]
        public virtual ShoppingCart ShoppingCart { get; set; }
      
        [ValidateNever]
        public virtual Product Product { get; set; }

        public CartItem() { }

    }
}
