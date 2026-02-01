using Microsoft.EntityFrameworkCore;
using StoreLibrary.DAL.Repositories;
using StoreLibrary.Models;

namespace jbH60Services.DAL
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly H60AssignmentDB_jbContext _context;
        public ShoppingCartRepository() { }
        public ShoppingCartRepository(H60AssignmentDB_jbContext context)
        {
            _context = context;
        }

        public ShoppingCart GetShoppingCartById(int id)
        {
            return _context.ShoppingCart.Where(x => x.CartId == id).First();
        }

        public DbSet<ShoppingCart> GetAllShoppingCarts()
        {

            
            return _context.ShoppingCart;
        }


        public ShoppingCart GetShoppingCartByEmail(string email)
        {
            return _context.ShoppingCart.Include(s => s.Customer).Where(x => x.Customer.Email == email).FirstOrDefault();
   
        }

        public CartItem GetExistingProduct(string email, int productId)
        {
        
            var existingCartItem = _context.ShoppingCart
                    .Where(cart => cart.Customer.Email == email)
                    .SelectMany(cart => cart.CartItems)
                .FirstOrDefault(cartItem => cartItem.ProductId == productId);

            return existingCartItem;
        }

        public void ModifyState(ShoppingCart shoppingCart)
        {
            _context.Entry(shoppingCart).State = EntityState.Modified;
        }

        public void SaveShoppingCart()
        {
            _context.SaveChangesAsync();
        }

        public void AddShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCart.Add(shoppingCart);

        }

        public void DeleteShoppingCart(ShoppingCart shoppingCart)
        {
            _context.ShoppingCart.Remove(shoppingCart);
        }

        public bool ShoppingCartExists(int id)
        {
            return (_context.ShoppingCart?.Any(e => e.CartId == id)).GetValueOrDefault();
        }

        public bool HasProducts(int cartId)
        {
            
            return (_context.CartItem.Any(c => c.CartId == cartId));
        }
    }
}
