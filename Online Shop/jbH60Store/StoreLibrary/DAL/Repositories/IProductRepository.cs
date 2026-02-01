using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreLibrary.Models;
using StoreLibrary.DTO;
using Microsoft.EntityFrameworkCore;

namespace StoreLibrary.DAL.Repositories
{
    public interface IProductRepository
    {    Dictionary<string, int> GetCategoryCount();

        IEnumerable<Product> ListProducts();
        IEnumerable<ProductDTO> SortByDescription();
        IEnumerable<ProductDTO> SortByProdCat();
        DbSet<ProductCategory> GetProductCategories();
        public IEnumerable<Product> SearchByStock(int num);
        public IEnumerable<Product> SearchBySellPrice(decimal price);
        public IEnumerable<Product> SearchByBuyPrice(decimal price);
        int GetProductCount();
        int ProductCountForProductCategory(int id);
        int SearchResultsCount(string productName);

        DbSet<Product> GetProductContext();

        Product GetProductById(int id);
        ProductDTO GetProductByIdDTO(int id);
       

        bool ProductExists(int id);
        void DeleteProduct(Product product);
        void Save();
        List<ProductDTO> AllProductsForProductCategory(int id);
        List<ProductDTO> PartialProductSearch(string productName);
        void AddProduct(Product product);

        void ModifyState(Product product);

        void DeleteProductImage(int id);
    }
}
