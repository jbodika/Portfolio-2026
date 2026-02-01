using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreLibrary.Models;
using StoreLibrary.DTO;

namespace StoreLibrary.DAL.Repositories
{
    public interface IProductCategoryRepository
    {
        DbSet <ProductCategory> GetProdCatContext();
        // ProductCategory GetProductCategory(int id);
        IEnumerable<ProductCategory> SortAlphabetically();
        void Save();
        void ModifyState(ProductCategory productCategory);
        int ProductCategoriesCount(int id);

        int CountOfProdCats(string prodCatName);

        int AllProductCategoriesCount();

        void AddProductCategory(ProductCategory productCategory);
        void DeleteProductCategory(ProductCategory productCategory);
        bool ProductCategoryExists(int id);

        List<ProductCategory> PartialProductCategorySearch(string prodCatName);

    }
}
