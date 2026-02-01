using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;
using StoreLibrary.DTO;

namespace jbH60Services.DAL
{
    public class CartItemRepository : ICartItemRepository
    {

        public CartItemRepository() { }

        private readonly H60AssignmentDB_jbContext _context;
        public CartItemRepository(H60AssignmentDB_jbContext context)
        {
            _context = context;
        }

        Task<ShoppingCart> ICartItemRepository.GetShoppingCartByIdAsync(int cartId)
        {
            throw new NotImplementedException();
        }

        public void AddCartItem(CartItem cartItem)
        {

            _context.CartItem.Add(cartItem);
        }



        public CartItem GetCartItemById(int cartItemId)
        {
            return _context.CartItem.Where(x => x.CartItemId == cartItemId).First();
        }

        public void DeleteCartItem(CartItem cartItem)
        {
            _context.CartItem.Remove(cartItem);
        }

        public void ModifyState(CartItem cartItem)
        {
            _context.Entry(cartItem).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public int GetCartItemsCount(string email)
        {
            return _context.CartItem.Include(x => x.ShoppingCart).ThenInclude(x => x.Customer).Where(x => x.ShoppingCart.Customer.Email == email).Sum(x => x.Quantity);
        }

        public List<CartItemDTO> GetAllCartItems(string email)
        {
            var cart = _context.CartItem.Select(c => new CartItemDTO { CartId = c.CartId, CartItemId = c.CartItemId, Price = c.Price, ProductId = c.ProductId, Quantity = c.Quantity, ShoppingCart = c.ShoppingCart, Product = c.Product }
                ).Where(x => x.ShoppingCart.Customer.Email == email).ToList();



            return cart;

        }
        public List<CartItem> GetProductsByCartItems(string email)
        {
            return _context.CartItem.Include(x => x.ShoppingCart).ThenInclude(x => x.Customer).Include(x => x.Product).Where(x => x.ShoppingCart.Customer.Email == email).Where(x => x.ProductId == x.Product.ProductId).ToList();
        }

        public decimal GetTotalPrice(string email)
        {
            return Math.Round(_context.CartItem.Include(x => x.ShoppingCart).ThenInclude(x => x.Customer).Include(x => x.Product).Where(x => x.ShoppingCart.Customer.Email == email).Where(x => x.ProductId == x.Product.ProductId).Sum(x => x.Price * x.Quantity), 2);
        }


        public bool CartItemExists(int id)
        {
            return (_context.CartItem?.Any(e => e.CartItemId == id)).GetValueOrDefault();
        }

        public Product GetProduct(int id)
        {
            return _context.Product.Where(x => x.ProductId == id).First();
        }

        public Product GetProductByCartItemId(int id)
        {

            return _context.CartItem.Where(x => x.CartItemId == id).Select(x => x.Product).First();
        }


        public void UpdatesOnQuantity(CartItem existingCartItem, CartItem cartItem)
        {

            if (cartItem.Quantity == 0)
            {
                DeleteCartItem(existingCartItem);
                var product = _context.Product.Where(x => x.ProductId == cartItem.ProductId).First();
                product.Stock += 1;

            }
            else if (cartItem.Quantity > 0)
            {
                int quantityChange = cartItem.Quantity - existingCartItem.Quantity;

                var product = _context.Product.Where(x => x.ProductId == cartItem.ProductId).First();
                product.Stock -= quantityChange;

                existingCartItem.Quantity = cartItem.Quantity;


            }

        }
    }
}
