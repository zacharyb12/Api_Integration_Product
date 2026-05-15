using Api_Integration_Product.Models.ProductModels;

namespace Api_Integration_Product.Services.ProductServices
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllAsync();

        Task<IEnumerable<ProductResponseDto>> GetAllByUserAsync(int userId);

        Task<ProductResponseDto> GetByIdAsync(int id);

        Task<ProductResponseDto> CreateAsync(CreateProductDto newProduct ,int id);

        Task<ProductResponseDto> UpdateAsync(UpdateProductDto updatedProduct,int idProduct,int userId);

        Task Delete(int productId , int userId);
    }
}