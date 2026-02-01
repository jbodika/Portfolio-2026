using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace jbH60Store.Models
{
    public class Customer
    {

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
         public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string Province {get; set; }
        public string? CreditCard { get; set; }
       
        [JsonIgnore] 
        [ValidateNever]
        public ICollection <Order> Orders { get; set; }
        
        [JsonIgnore]
        [ValidateNever]
        public virtual ShoppingCart ShoppingCarts { get; set; }

        public Customer() { }
    }
}
