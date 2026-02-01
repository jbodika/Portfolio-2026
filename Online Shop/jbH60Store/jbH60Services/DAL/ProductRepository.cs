using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
using StoreLibrary.DTO;

namespace jbH60Services.DAL
{
    public class ProductRepository : IProductRepository
    {

        public ProductRepository() { }

        private readonly H60AssignmentDB_jbContext _context;
        public ProductRepository(H60AssignmentDB_jbContext context)
        {
            _context = context;
        }

        public IEnumerable<ProductDTO> SortByDescription()
        {
            return _context.Product
        .Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            ProdCatId = p.ProdCatId,
            Stock = p.Stock,
            SellPrice = p.SellPrice,
            Description = p.Description,
            Manufacturer = p.Manufacturer,
            ProdCat = p.ProdCat,
                Image = p.Image
        }).OrderBy(x => x.Description)
        .ToList();

        }
        public IEnumerable<ProductDTO> SortByProdCat()
        {
            return _context.Product
        .Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            ProdCatId = p.ProdCatId,
            Stock = p.Stock,
            SellPrice = p.SellPrice,
            Manufacturer = p.Manufacturer,
            Description = p.Description,
            ProdCat = p.ProdCat,
            Image = p.Image

       
        }).OrderBy(x => x.ProdCat.ProdCat).ThenBy(x => x.Description)
        .ToList();

        }

        public IEnumerable<Product> ListProducts()
        {
            return _context.Product.Include(p => p.ProdCat).OrderBy(x => x.Description).ToList();
        }



        public IEnumerable<Product> SearchByStock(int num)
        {
            return _context.Product.Where(x => x.Stock == num);

        }
        public IEnumerable<Product> SearchByBuyPrice(decimal price)
        {
            return _context.Product.Where(x => x.BuyPrice == price);
        }
        public IEnumerable<Product> SearchBySellPrice(decimal price)
        {
            return _context.Product.Where(x => x.SellPrice == price);

        }

        public Product GetProductById(int id)
        {
      
         return _context.Product.Include(p => p.ProdCat).First(m => m.ProductId == id);
        }

        public ProductDTO GetProductByIdDTO(int id)
        {
            return _context.Product
                .Select(p => new ProductDTO
                {
                    ProductId = p.ProductId,
                    ProdCatId = p.ProdCatId,
                    Stock = p.Stock,
                    SellPrice = p.SellPrice,
                    BuyPrice = p.BuyPrice,
                    EmployeeNotes = p.EmployeeNotes,
                    Manufacturer = p.Manufacturer,
                    Description = p.Description,
                    ProdCat = p.ProdCat,
                    Image = p.Image
                }).Where(x => x.ProductId == id).First();
         }

        bool IProductRepository.ProductExists(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteProduct(Product product)
        {
            _context.Product.Remove(product);
        }

        public void Save()
        {
            _context.SaveChangesAsync();
        }

        public void AddProduct(Product product)
        {
            _context.Product.Add(product);
        }

        public void ModifyState(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
        public Dictionary<string, int> GetCategoryCount()
        {
            Dictionary<string, int> categoryCount;
            var sortedProducts = SortByDescription();

            categoryCount = sortedProducts
                .GroupBy(p => p.ProdCat.ProdCat)
                .ToDictionary(g => g.Key, g => g.Count());

            return categoryCount;
        }
        public void DeleteProductImage(int id)
        {
            var product = GetProductById(id);
            if (product != null)
            {
               
                product.Image = null; 

           
            }
        }

        public List<ProductDTO> AllProductsForProductCategory(int id)
        {

            return _context.Product.Where(x => x.ProdCatId == id).Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProdCatId = p.ProdCatId,
                Stock = p.Stock,
                SellPrice = p.SellPrice,
                Manufacturer = p.Manufacturer,
                Description = p.Description,
                ProdCat = p.ProdCat,
                Image = p.Image
                

            }).OrderBy(x => x.ProdCat.ProdCat).ThenBy(x => x.Description)
               .ToList();

        }

        public int ProductCountForProductCategory(int id)
        {
            return AllProductsForProductCategory(id).Count();
        }

        public List<ProductDTO> PartialProductSearch(string productName)
        {
            return _context.Product.Where(product => product.Description.Contains(productName))
            .Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProdCatId = p.ProdCatId,
                Stock = p.Stock,
                SellPrice = p.SellPrice,
                Manufacturer = p.Manufacturer,
                Description = p.Description,
                ProdCat = p.ProdCat,
                Image = p.Image
            }).OrderBy(x => x.Description)
              .ToList();



        }
        public int GetProductCount()
        {
            return SortByDescription().Count();
        }

        public int SearchResultsCount(string productName)
        {
            return PartialProductSearch(productName).Count();
        }
        public DbSet<ProductCategory> GetProductCategories()
        {
            return _context.ProductCategory;
        }
        public DbSet<Product> GetProductContext()

        {
            return _context.Product;

        }


        public void Update(Product product)
        {
            _context.Product.Update(product);
        }







    }
}
