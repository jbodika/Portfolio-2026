using System;
using System.Collections.Generic;

namespace jbH60Customer.Models
{
    public  class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            OrderItems = new HashSet<OrderItem>();
        }

        public int ProductId { get; set; }
        public int ProdCatId { get; set; }
        public string Description { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public int Stock { get; set; }
        public decimal? BuyPrice { get; set; }
        public decimal? SellPrice { get; set; }
        public string? EmployeeNotes { get; set; }
        public byte[]? Image { get; set; }

        public virtual ProductCategory ProdCat { get; set; } = null!;
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
