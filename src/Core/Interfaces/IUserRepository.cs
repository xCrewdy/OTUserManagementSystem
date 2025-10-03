using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OTUserManagementSystem.src.Core.Models;

namespace OTUserManagementSystem.src.Core.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<List<User>> GetUsersLoggedInSinceAsync(DateTime sinceUtc);
    }
}
