using ApiJwtEfOracle.Models;

namespace ApiJwtEfOracle.Repositories
{
    public interface IAuthRepository
    {
        Task<int> RegisterAsync(string username, string password);
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
