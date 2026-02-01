using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace StoreLibrary.Models
{
    public class ShoppingCart
    {

        public int CartId { get; set; }

        public int CustomerId { get; set; }
       
        public DateTime DateCreated { get; set; }
        [JsonIgnore]
        [ValidateNever]

        public virtual Customer Customer { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public ICollection <CartItem> CartItems { get; set; }
        public ShoppingCart() {  }
    }
}
