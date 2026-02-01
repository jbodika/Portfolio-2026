using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace StoreLibrary.Models
{
    public class Order
    {
        public int OrderId { get; set; }
    
        public int CustomerId { get; set; }
    
        public DateTime DateCreated { get; set; }
        public DateTime DateFufilled { get; set; }
        public decimal Total { get; set; }
        public decimal Taxes { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual Customer Customer{ get; set; }
        [JsonIgnore]
              [ValidateNever]
        public ICollection <OrderItem> OrderItems { get; set; }
        public Order() { }
    }
}
