using Humanizer;
using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.DTO;
using StoreLibrary.Models;

namespace jbH60Services.DAL

{
    public class ProductCategoryRepository :IProductCategoryRepository
    {
        private readonly H60AssignmentDB_jbContext _context;
        public ProductCategoryRepository(H60AssignmentDB_jbContext context)
        {
            _context = context;

        }

        public DbSet <ProductCategory> GetProdCatContext()
        {
            return _context.ProductCategory;
        }

        public void ModifyState(ProductCategory productCategory)
        {
            _context.Entry(productCategory).State = EntityState.Modified;

        }
        public void Save()
        {
            _context.SaveChangesAsync();
        }
        public List<ProductCategory> PartialProductCategorySearch(string prodCatName)
        {
            return _context.ProductCategory.Where(product => product.ProdCat.Contains(prodCatName)).OrderBy(x => x.ProdCat).ToList();
        }

        public int ProductCategoriesCount(int id)
        {
       
                return _context.Product.Where(x=>x.ProdCatId==id).Count();
            

        }

        public IEnumerable<ProductCategory> SortAlphabetically()
        {
            return _context.ProductCategory.OrderBy(x => x.ProdCat);
        }

        public void AddProductCategory(ProductCategory productCategory) {
        _context.ProductCategory.Add(productCategory);
        }
        public void DeleteProductCategory(ProductCategory productCategory)
        {
            _context.Product.RemoveRange(_context.Product.Where(x => x.ProdCatId == productCategory.CategoryId));
            _context.ProductCategory.Remove(productCategory);
        }

        public bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategory?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }

        public int AllProductCategoriesCount()
        {
          return _context.ProductCategory.Count();
        }

        public int CountOfProdCats(string prodCatName)
        {
            return _context.ProductCategory.Where(x => x.ProdCat.Contains(prodCatName)).Count();
        }


        //public async Task <ProductCategory> GetProductCategory(int id)
        //{
        //    return _context.ProductCategories.FindAsync(id);
        //}


    }
}
