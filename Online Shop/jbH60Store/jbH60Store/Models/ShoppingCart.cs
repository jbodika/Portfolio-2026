namespace jbH60Store.Models
{
    public class ShoppingCart
    {

        public int CartId { get; set; }

        public int CustomerId { get; set; }
       
        public DateTime DateCreated { get; set; }
        public virtual Customer Customer { get; set; }

        public ICollection <CartItem> CartItem { get; set; }
        public ShoppingCart() {  }
    }
}
