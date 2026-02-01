using StoreLibrary.Models;

namespace StoreLibrary.DTO
{
    public class ProductDTO
    {
        public int ProductId { get; set; }
        public int ProdCatId { get; set; }
        public string Description { get; set; }
        public string? Manufacturer { get; set; }
        public int Stock {  get; set; }
        public decimal? BuyPrice { get; set; }
        public decimal? SellPrice {  get; set; }
        public string? EmployeeNotes { get; set; }
        public byte[] Image { get; set; }
        public virtual ProductCategory ProdCat { get; set; } = null!;

    }
}
