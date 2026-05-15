using Api_Integration_Product.Models.AuthModels;

namespace Api_Integration_Product.Services.AuthServices
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO form);
        Task<AuthResponseDTO> LoginAsync(LoginDTO form);
    }
}