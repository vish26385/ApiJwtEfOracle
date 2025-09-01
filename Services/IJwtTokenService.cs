using ApiJwtEfOracle.Models;

namespace ApiJwtEfOracle.Services
{
    public interface IJwtTokenService
    {
        (string token, DateTime expiresAt) CreateToken(User user);
    }
}
