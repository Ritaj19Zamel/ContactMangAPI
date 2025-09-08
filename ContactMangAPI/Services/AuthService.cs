using ContactMangAPI.Data;
using ContactMangAPI.DTOs;
using ContactMangAPI.Models;
using ContactMangAPI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactMangAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JWT _jwt;
        public AuthService(ApplicationDbContext context, IOptions<JWT> jwt)
        {
            _context = context;
            _jwt = jwt.Value;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(User user)
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email)

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: creds
            );

            return token;
        }
        public async Task<ApiResponse> LoginAsync(LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Email) ||
                 string.IsNullOrWhiteSpace(model.Password))
            {
                return new ApiResponse { Success = false, Message = "Email and password are required" };
            }
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            var hasher = new PasswordHasher<User>();
            if (user == null || hasher.VerifyHashedPassword(user, user.Password, model.Password) == PasswordVerificationResult.Failed)
            {
                return new ApiResponse { Success = false, Message = "Invalid email or password" };
            }
            var token = await CreateJwtToken(user);
            return new ApiResponse
            {
                Success = true,
                Message = "Login successful",
                Data = new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Name = user.FullName
                }
            };
        }
        public async Task<ApiResponse> RegisterAsync(RegisterDto model)
        {
            if(string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.FullName))
            {
                return new ApiResponse { Success = false, Message = "All fields are required" };
            }
           
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return new ApiResponse { Success = false, Message = "Email is already registered" };
            }
            if (model.Password.Length < 6)
            {
                return new ApiResponse { Success = false, Message = "Password must be at least 6 characters long" };
            }
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email
            };
            var hasher = new PasswordHasher<User>();
            user.Password = hasher.HashPassword(user, model.Password);
            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                var jwtToken = await CreateJwtToken(user);
                return new ApiResponse
                {
                    Success = true,
                    Message = "User registered successfully",
                    Data = new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        Name = user.FullName
                    }
                };
            }
            return new ApiResponse { Success = false, Message = "Registration failed" };
        }
    }
}
