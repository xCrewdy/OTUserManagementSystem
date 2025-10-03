using OTUserManagementSystem.src.Core.Models;

namespace OTUserManagementSystem.src.Core.Interfaces
{
    public interface IJwtTokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(User user);
    }
}
