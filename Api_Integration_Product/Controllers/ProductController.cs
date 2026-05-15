using Api_Integration_Product.Models.ProductModels;
using Api_Integration_Product.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api_Integration_Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IProductService _service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]CreateProductDto newProduct)
        {
            try
            {
                int userId = GetCurrentUserId();
                var product = await _service.CreateAsync(newProduct , userId);

                return Ok(product);

            }catch(Exception)
            {
                return BadRequest("Il y a eu une erreur lors de l'ajout veuillez reesayer !");
            }

        }

        // Private 

        private int GetCurrentUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("UserID introuvable");

            int valueInt = -1;

            int.TryParse(value , out valueInt);

            return valueInt;
            

        }

    }
}
