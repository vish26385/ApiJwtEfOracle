using ApiJwtEfOracle.DTOs;
using ApiJwtEfOracle.Repositories;
using ApiJwtEfOracle.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiJwtEfOracle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IJwtTokenService _jwtTokenService;
        public AccountController(IAuthRepository authRepository, IJwtTokenService jwtTokenService)
        {
            _authRepository = authRepository;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            if(string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
                return BadRequest("Username and password are required.");

            var id = await _authRepository.RegisterAsync(req.Username, req.Password);
            return Ok(new { success = true, UserId = id } );
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            if (string.IsNullOrEmpty(req.Username) || string.IsNullOrEmpty(req.Password))
                return BadRequest("Username and password are required.");

            var user = await _authRepository.AuthenticateAsync(req.Username, req.Password);
            if (user == null) return Unauthorized( new { success = false, message = "Invalid username or password." });

            var (token, exp) = _jwtTokenService.CreateToken(user);
            var refreshtoken = await _jwtTokenService.CreateAndStoreRefreshTokenAsync(user);
            return Ok(new AuthResponse{ Token = token, ExpiresAt = exp, Username = user.Username, RefreshToken = refreshtoken });
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(TokenRequest request)
        {
            var user = await _jwtTokenService.CheckRefreshTokenAsync(request.RefreshToken);

            if (user == null)
                return Unauthorized("Invalid refresh token");

            var (token, exp) = _jwtTokenService.CreateToken(user);
            var newRefreshToken = await _jwtTokenService.CreateAndStoreRefreshTokenAsync(user);
            return Ok(new AuthResponse { Token = token, ExpiresAt = exp, Username = user.Username, RefreshToken = newRefreshToken });
        }
    }
}
