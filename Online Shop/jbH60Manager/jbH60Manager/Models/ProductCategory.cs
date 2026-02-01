using jbH60Manager.Models;
using System.ComponentModel;

namespace jbH60Manager.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
       
        }

  
    
        [DisplayName("Product Category Id")]
        public int CategoryId { get; set; }
        [DisplayName("Product Category Name")]

        public string ProdCat { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }


    }
}
