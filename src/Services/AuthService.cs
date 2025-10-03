using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OTUserManagementSystem.src.Core.Interfaces;
using OTUserManagementSystem.src.Core.Models;

namespace OTUserManagementSystem.src.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IJwtTokenService _jwt;
        private readonly IWebhookDispatcher _webhook;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository repo,
            IJwtTokenService jwt,
            IWebhookDispatcher webhook,
            ILogger<AuthService> logger)
        {
            _repo = repo;
            _jwt = jwt;
            _webhook = webhook;
            _logger = logger;
        }

        public async Task<(bool success, string? error)> RegisterAsync(string username, string email, string password, string? firstName, string? lastName)
        {
            if (await _repo.GetByUsernameAsync(username) != null)
                return (false, "Username already taken");
            if (await _repo.GetByEmailAsync(email) != null)
                return (false, "Email already registered");

            var user = new User
            {
                Username = username,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.Now
            };

            await _repo.AddAsync(user);
            _logger.LogInformation("New user registered: {Username} ({Email})", username, email);
            return (true, null);
        }

        public async Task<(bool success, string? token, DateTime? expiresAt, string? error)> LoginAsync(string usernameOrEmail, string password)
        {
            User? user = null;
            if (usernameOrEmail.Contains("@"))
                user = await _repo.GetByEmailAsync(usernameOrEmail);
            else
                user = await _repo.GetByUsernameAsync(usernameOrEmail);

            if (user == null)
            {
                _logger.LogWarning("Login failed for unknown identity: {Identity}", usernameOrEmail);
                return (false, null, null, "Invalid credentials");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed for user {Username}: invalid password", user.Username);
                return (false, null, null, "Invalid credentials");
            }

            user.LastLoginAt = DateTime.Now;
            await _repo.UpdateAsync(user);

            var tokenResult = _jwt.GenerateToken(user);

            _logger.LogInformation("User logged in: {Username}", user.Username);

            await _webhook.DispatchLoginEventAsync(user);
            return (true, tokenResult.Token, tokenResult.ExpiresAt, null);
        }
    }
}
