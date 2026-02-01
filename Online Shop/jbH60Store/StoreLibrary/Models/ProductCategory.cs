using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL;
using System;

using System.Collections.Generic;
using System.ComponentModel;

namespace StoreLibrary.Models
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            Products = new HashSet<Product>();
       
        }

        private readonly H60AssignmentDB_jbContext _context;

        public ProductCategory(H60AssignmentDB_jbContext context)
        {
            _context = context;
        }
        [DisplayName("Product Category Id")]
        public int CategoryId { get; set; }
        [DisplayName("Product Category Name")]

        public string ProdCat { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }



        public async Task CreateProductCategory(H60AssignmentDB_jbContext _context, [Bind("CategoryId,ProdCat")] ProductCategory productCategory)
        {
            _context.Add(productCategory);
             await _context.SaveChangesAsync();
        }



        public async Task<ProductCategory> FindProductCategory(H60AssignmentDB_jbContext _context, int? id)
        {
            if ( _context.ProductCategory == null)
           {
                throw new InvalidOperationException("Entity set 'H60AssignmentDB_jbContext.ProductCategories' is null.");
            }


            return await _context.ProductCategory
              .Include(pc => pc.Products)
              .FirstOrDefaultAsync(pc => pc.CategoryId == id);

        }

        public int GetProductCount()
        {
            return this.Products.Count;
        }




        public async Task DeleteProductCategory(H60AssignmentDB_jbContext _context, int? id)
        {
            if (_context.ProductCategory == null)
            {
                throw new InvalidOperationException("Entity set 'H60AssignmentDB_jbContext.ProductCategories' is null.");
            }
            var productCategory = await _context.ProductCategory.FindAsync(id);

           if (productCategory != null)
           {
                _context.Product.RemoveRange(_context.Product.Where(x => x.ProdCatId == productCategory.CategoryId));
               
                _context.ProductCategory.Remove(productCategory);
            }

           await _context.SaveChangesAsync();
        
        }

        public async Task<ProductCategory> FindEditAsync(H60AssignmentDB_jbContext _context, int? id)
        {
            if (_context.ProductCategory == null)
            {
                throw new InvalidOperationException("Entity set 'H60AssignmentDB_jbContext.ProductCategories' is null.");
            }
            return await _context.ProductCategory.FindAsync(id);
        }

        public List<ProductCategory> AllProductsSortedByCategoryName(H60AssignmentDB_jbContext _context)
        {
           return _context.ProductCategory.OrderBy(x => x.ProdCat).ToList();
        }


    

       
        public bool ProductCategoryExists(int id,H60AssignmentDB_jbContext _context)
        {
            return (_context.ProductCategory?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }


    }
}
