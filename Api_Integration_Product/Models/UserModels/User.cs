using Api_Integration_Product.Models.CartItemmodels;
using Api_Integration_Product.Models.ProductModels;

namespace Api_Integration_Product.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public ICollection<CartItem> Cart { get; set; } = new List<CartItem>();

    }
}
