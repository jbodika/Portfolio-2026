using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreLibrary.Models
{
    public class CartItem
    {

        public int CartItemId { get; set; }  
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }   
        public decimal Price { get; set;}
        [JsonIgnore]
        [ValidateNever]
        public virtual ShoppingCart ShoppingCart { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual Product Product { get; set; }
      
        public CartItem() { }

    }
}
