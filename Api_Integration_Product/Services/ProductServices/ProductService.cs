using Api_Integration_Product.Data;
using Api_Integration_Product.Models.ProductModels;
using Microsoft.EntityFrameworkCore;

namespace Api_Integration_Product.Services.ProductServices
{
    public class ProductService(MyAppContext _context) : IProductService
    {
        public async Task<ProductResponseDto> CreateAsync(CreateProductDto newProduct, int userId)
        {
            Product productToAdd = new()
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Stock = newProduct.Stock,
                UserId = userId
            };

            _context.Products.Add(productToAdd);
            await _context.SaveChangesAsync();

            return ToDto(productToAdd);
        }
        public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
        {
            return await _context.Products.Select(p => ToDto(p)).ToListAsync();
        }


        public Task Delete(int productId, int userId)
        {
            throw new NotImplementedException();
        }


        public Task<IEnumerable<ProductResponseDto>> GetAllByUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponseDto> UpdateAsync(UpdateProductDto updatedProduct, int idProduct, int userId)
        {
            throw new NotImplementedException();
        }


        // Private 

        private static ProductResponseDto ToDto(Product p)
        {
            return new ProductResponseDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                UserId = p.UserId
            };

        }
    }
}
