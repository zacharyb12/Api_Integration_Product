using Api_Integration_Product.Models.UserModels;

namespace Api_Integration_Product.Models.ProductModels
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        
        public int Stock { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
