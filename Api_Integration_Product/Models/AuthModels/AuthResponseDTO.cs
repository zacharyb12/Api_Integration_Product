namespace Api_Integration_Product.Models.AuthModels
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
