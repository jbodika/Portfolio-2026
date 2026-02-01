using System.ComponentModel.DataAnnotations.Schema;

namespace jbH60Store.Models
{
    public class CartItem
    {

        public int CartItemId { get; set; }  
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }   
        public decimal Price { get; set;}
        public virtual ShoppingCart ShoppingCart { get; set; } 
       
        public virtual Product Product { get; set; }
        
    
        public CartItem() { }

    }
}
