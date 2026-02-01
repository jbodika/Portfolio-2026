using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace StoreLibrary.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }

       public int Quantity { get; set; }
        public decimal Price { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual Order Order { get; set; }
        [JsonIgnore]
        [ValidateNever]

        public virtual Product Product { get; set; }

        public OrderItem() { }
    }
}
