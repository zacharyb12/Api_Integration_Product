using Api_Integration_Product.Data;
using Api_Integration_Product.Models.UserModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Integration_Product.Services.AuthServices
{
    public class AuthService(MyAppContext _context,IConfiguration _configuration) : IAuthService
    {

        // Register

        // Login
        
        // GenerateToken
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.Username),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role , user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer : _configuration["Jwt:Issuer"],
                    audience : _configuration["Jwt:Audience"],
                    claims : claims,
                    expires : DateTime.Now.AddHours(1),
                    signingCredentials : creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }   


        // BuildResponse
    }
}
