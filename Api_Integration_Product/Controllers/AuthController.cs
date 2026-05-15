using Api_Integration_Product.Models.AuthModels;
using Api_Integration_Product.Services.AuthServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_Integration_Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService _service) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO form)
        {
            try
            {
                var response = await _service.RegisterAsync(form);

                return Ok(response);

            }catch(InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message});
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO form)
        {
            try
            {
                var response = await _service.LoginAsync(form);

                return Ok(response);

            }catch(UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}