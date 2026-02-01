
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace jbH60Manager.Models
{
    public partial class Product
    {


        [DisplayName("Product Id")]
        public int ProductId { get; set; }

        [DisplayName("Product Category Id")]
        public int ProdCatId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(80, ErrorMessage = "Product Name must be between 1 to 80 characters")]
        [DisplayName("Product Name")]
        public string Description { get; set; }

        [StringLength(80, ErrorMessage = "Manufacturer Name must be between 1 to 80 characters")]
        [Required(ErrorMessage = "Manufacturer Name is required")]

        [DisplayName("Manufacturer Name")]
        public string? Manufacturer { get; set; }

        [Required(ErrorMessage = "Amount in stock is required")]
        [DisplayName("Amount in stock")]
      
        public int Stock { get; set; } // Use int? to make it nullable

        [Required(ErrorMessage = "Buy price is required")]
   
        [DisplayName("Buy price")]
        public decimal? BuyPrice { get; set; } // Use decimal? to make it nullable

        [DisplayName("Selling price")]

        public decimal? SellPrice { get; set; } // Use decimal? to make it nullable

        [DisplayName("Notes for employees")]
        [DataType(DataType.MultilineText)]
        public string? EmployeeNotes { get; set; }

        public byte[]? Image { get; set; }

        //public virtual ICollection<OrderItem> OrderItems { get; set; }
  


        //public virtual ICollection<CartItem> CartItems { get; set; }

   
        [DisplayName("Product Category Name")]
        public virtual ProductCategory ProdCat { get; set; } = null!;


        //public Product()
        //{
        //    CartItems = new HashSet<CartItem>();
        //    OrderItems = new HashSet<OrderItem>();
        //}


        public Product(int productId, int prodCatId, string? description, string? manufacturer, int stock, decimal? buyPrice, decimal? sellPrice, string? employeeNotes, ProductCategory prodCat)
        {
            ProductId = productId;
            ProdCatId = prodCatId;
            Description = description;
            Manufacturer = manufacturer;
            Stock = stock;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
            ProdCat = prodCat;
            EmployeeNotes = employeeNotes;
        }




    }
}
