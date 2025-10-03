using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using OTUserManagementSystem.src.Core.Interfaces;
using OTUserManagementSystem.src.Services;

namespace OTUserManagementSystem.src.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService auth, ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Core.DTOs.RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { error = "Username, email and password are required." });

            var (success, error) = await _auth.RegisterAsync(req.Username, req.Email, req.Password, req.FirstName, req.LastName);
            if (!success) return Conflict(new { error });

            return CreatedAtAction(null, new { message = "User created" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Core.DTOs.LoginRequest req)
        {
            var (success, token, expiresAt, error) = await _auth.LoginAsync(req.UsernameOrEmail, req.Password);
            if (!success) return Unauthorized(new { error });

            return Ok(new Core.DTOs.AuthResponse
            {
                Token = token!,
                ExpiresAt = expiresAt!.Value,
                Username = req.UsernameOrEmail
            });
        }
    }
}
