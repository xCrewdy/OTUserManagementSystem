using System;
using System.Threading.Tasks;
using OTUserManagementSystem.src.Core.Models;

namespace OTUserManagementSystem.src.Core.Interfaces
{
    public interface IAuthService
    {
        Task<(bool success, string? error)> RegisterAsync(string username, string email, string password, string firstName, string lastName);
        Task<(bool success, string? token, DateTime? expiresAt, string? error)> LoginAsync(string usernameOrEmail, string password);
    }
}
