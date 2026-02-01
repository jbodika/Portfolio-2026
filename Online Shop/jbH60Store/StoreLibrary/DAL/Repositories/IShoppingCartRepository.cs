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
    public interface IShoppingCartRepository
    {
        DbSet<ShoppingCart> GetAllShoppingCarts();
        ShoppingCart GetShoppingCartById(int id);
        void ModifyState(ShoppingCart shoppingCart);
        void SaveShoppingCart();
        void AddShoppingCart(ShoppingCart shoppingCart);
        void DeleteShoppingCart(ShoppingCart shoppingCart);
        bool ShoppingCartExists(int id);
        ShoppingCart GetShoppingCartByEmail(string email);
        CartItem GetExistingProduct(string email, int productId);
        bool HasProducts(int cartId);

    }
}
