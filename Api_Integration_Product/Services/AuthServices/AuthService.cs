using Api_Integration_Product.Data;
using Api_Integration_Product.Models.AuthModels;
using Api_Integration_Product.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Integration_Product.Services.AuthServices
{
    public class AuthService(MyAppContext _context,IConfiguration _configuration) : IAuthService
    {

        // Register
        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO form)
        {
            if(await _context.Users.AnyAsync(u => u.Email == form.Email))
            {
                throw new InvalidOperationException("Email déja utilisé!");
            }

            if (await _context.Users.AnyAsync(u => u.Username == form.Username))
            {
                throw new InvalidOperationException("Ce nom d'utilisateur est déja utilisé!");
            }

            User newUser = new()
            {
                Username = form.Username,
                Email = form.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(form.Password),
                Role = "User"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return BuildResponse(newUser);
        }

        // Login
        public async Task<AuthResponseDTO> LoginAsync(LoginDTO form)
        {
            User? u = await _context.Users.FirstOrDefaultAsync(u => u.Email == form.Email);

            if(u == null || !BCrypt.Net.BCrypt.Verify(form.Password,u.PasswordHash))
            {
                throw new UnauthorizedAccessException("Email ou mot de passe invalide");
                //throw new Exception();
            }

            return BuildResponse(u);
        }

        // =================================================================
        // Les methodes ci dessous ne seront utilisés que dans cette classe
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
        private AuthResponseDTO BuildResponse(User user)
        {
            return new AuthResponseDTO()
            {
                Token = GenerateToken(user),
                Username = user.Username,
                Role = user.Role,
            };
        }
    }
}
