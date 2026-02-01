using System;
using System.Collections.Generic;

namespace jbH60Customer.Models
{
    public  class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
        }

        public int CategoryId { get; set; }
        public string ProdCat { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
