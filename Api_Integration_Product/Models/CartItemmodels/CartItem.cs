using Api_Integration_Product.Models.ProductModels;
using Api_Integration_Product.Models.UserModels;

namespace Api_Integration_Product.Models.CartItemmodels
{
    public class CartItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int ProductId  { get; set; }

        public Product? Product { get; set; }
    }
}
