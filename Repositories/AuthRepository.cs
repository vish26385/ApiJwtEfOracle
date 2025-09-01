using ApiJwtEfOracle.Data;
using ApiJwtEfOracle.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;
using System.Text;

namespace ApiJwtEfOracle.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        private readonly string _connectionstring;
        public AuthRepository(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _connectionstring = configuration.GetConnectionString("ConnectionString");
        }
        public async Task<int> RegisterAsync(string username, string password)
        {
            CreatePasswordHash(password, out var hash, out var salt);

            await using var con = new OracleConnection(_connectionstring);
            if (con.State != System.Data.ConnectionState.Open) await con.OpenAsync();

            var p = new DynamicParameters();
            p.Add(":p_username", username);
            p.Add(":p_password_hash", hash);
            p.Add(":p_password_salt", salt);
            p.Add(":p_new_user_id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            await con.ExecuteAsync("PKG_AUTH.CREATE_USER", p, commandType: System.Data.CommandType.StoredProcedure);
            return p.Get<int>(":p_new_user_id");
        }

        private static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            await using var con = new OracleConnection(_connectionstring);
            if (con.State != System.Data.ConnectionState.Open) await con.OpenAsync();

            var p = new DynamicParameters();
            p.Add(":p_username", username);
            p.Add(":p_password", password);            
            p.Add(":p_user_id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            p.Add(":p_out_success", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            await con.ExecuteAsync("PKG_AUTH.AUTHENTICATE_USER", p, commandType: System.Data.CommandType.StoredProcedure);

            var ok = p.Get<int>(":p_out_success") == 1;
            if (!ok) return null;

            var userId = p.Get<int>(":p_user_id");
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
