using Microsoft.EntityFrameworkCore;
using StoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreLibrary.DTO;
namespace StoreLibrary.DAL.Repositories
{
    public interface ICartItemRepository
    {
        Task<ShoppingCart> GetShoppingCartByIdAsync(int cartId);
        List<CartItemDTO> GetAllCartItems(string email);
        int GetCartItemsCount(string email);
        Product GetProduct(int id);

        Product GetProductByCartItemId(int id);
        decimal GetTotalPrice(string email);

        void UpdatesOnQuantity(CartItem existingCartItem, CartItem newCartItem);
        void AddCartItem(CartItem cartItem);
       
        void DeleteCartItem(CartItem cartItem);
        public bool CartItemExists(int id);
        void Save();
        CartItem GetCartItemById(int cartItemId);


        void ModifyState(CartItem cartItem);

    }
}
