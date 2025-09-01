using ApiJwtEfOracle.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiJwtEfOracle.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }    

        public (string token, DateTime expiresAt) CreateToken(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var expires = DateTime.Now.AddMinutes(int.Parse(jwtSection["ExpiresMinutes"]!));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id.ToString())
            };   

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expires);
        }
    }
}   
