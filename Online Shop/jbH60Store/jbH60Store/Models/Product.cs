using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using jbH60Store.Models;
using System.ComponentModel.DataAnnotations;

namespace jbH60Store.Models
{
    public partial class Product
    {


        [DisplayName("Product Id")]
        public int ProductId { get; set; }

        [DisplayName("Product Category Id")]
        public int ProdCatId { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Product Name is required")]
        [StringLength(80, ErrorMessage = "Product Name must be between 1 to 80 characters")]
        [DisplayName("Product Name")]
        public string Description { get; set; }

        [StringLength(80, ErrorMessage = "Manufacturer Name must be between 1 to 80 characters")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Manufacturer Name is required")]

        [DisplayName("Manufacturer Name")]
        public string? Manufacturer { get; set; }

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Amount in stock is required")]
        [DisplayName("Amount in stock")]
        [PositiveDecimal]
        public int Stock { get; set; } // Use int? to make it nullable

        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Buy price is required")]
        [ValidDecimal(ErrorMessage = "New Sell Buy is not a valid decimal.")]
        [PositiveDecimal]
        [DisplayName("Buy price")]
        public decimal? BuyPrice { get; set; } // Use decimal? to make it nullable

        [DisplayName("Selling price")]
        [ValidDecimal(ErrorMessage = "New Sell Price is not a valid decimal.")]
        [PositiveDecimal]
        [SellPriceGreaterOrEqual]
        public decimal? SellPrice { get; set; } // Use decimal? to make it nullable

        [DisplayName("Notes for employees")]
        [DataType(DataType.MultilineText)]
        public string? EmployeeNotes { get; set; }

        public byte[]? Image { get; set; }


        [ValidateNever]


        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [ValidateNever]


        public virtual ICollection<CartItem> CartItems { get; set; }

        [ValidateNever]
        [DisplayName("Product Category Name")]
        public virtual ProductCategory ProdCat { get; set; } = null!;


        public Product()

        {

        }




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

        public List<Product> ListOfProducts(H60AssignmentDB_jbContext _context, out Dictionary<string, int> categoryCounts)
        {
            var productList = _context.Products.Include(p => p.ProdCat).OrderBy(x => x.Description).ToList();

            categoryCounts = productList
                .GroupBy(p => p.ProdCat.ProdCat)
                .ToDictionary(g => g.Key, g => g.Count());

            return productList;
        }


        public List<Product> ViewByProductCategory(H60AssignmentDB_jbContext _context, int id)
        {
            return _context.Products
                .Where(product => product.ProdCatId == id)
                .Include(x => x.ProdCat)
                .OrderBy(product => product.ProdCat.ProdCat)
                .ThenBy(product => product.Description)
                .ToList();
        }

        public List<Product> PartialProductSearch(H60AssignmentDB_jbContext _context, string productName)
        {

            return _context.Products
               .Where(product => product.Description.Contains(productName))
               .Include(x => x.ProdCat)
               .OrderBy(product => product.Description)
               .ToList();


        }


        //public List<Product> GetAllProductsSortedByCategoryAndProduct(H60AssignmentDB_jbContext _context)
        //{
        //    return _context.Products
        //        .Include(x => x.ProdCat)
        //        .OrderBy(product => product.ProdCat.ProdCat) // Sort by category first
        //        .ThenBy(product => product.Description)       // Sort by product name within each category
        //        .ToList();
        //}


        //public List<ProductCategory> GetProductList(H60AssignmentDB_jbContext _context)
        //{
        //    return _context.ProductCategories.OrderBy(x => x.ProdCat).ToList();
        //}


        //public List<ProductCategory> BuyPriceUpdate(H60AssignmentDB_jbContext _context)
        //{
        //    return _context.ProductCategories.OrderBy(x => x.ProdCat).ToList();
        //}

        //public List<ProductCategory> SellPriceUpdate(H60AssignmentDB_jbContext _context)
        //{
        //    return _context.ProductCategories.OrderBy(x => x.ProdCat).ToList();
        //}


        public async Task<Product> FindProductId(H60AssignmentDB_jbContext _context, int? id)
        {
            return await _context.Products.Include(p => p.ProdCat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
        }


        public bool ProductExists(int id, H60AssignmentDB_jbContext _context)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
