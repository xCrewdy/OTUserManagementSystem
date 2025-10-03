using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OTUserManagementSystem.src.Core.Models;
using OTUserManagementSystem.src.Infrastructure.Data;
using OTUserManagementSystem.src.Core.Interfaces;

namespace OTUserManagementSystem.src.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext db) => _db = db;
        public async Task AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetAllAsync() =>
            await _db.Users.ToListAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await _db.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _db.Users.FindAsync(id);

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsersLoggedInSinceAsync(DateTime sinceUtc) =>
            await _db.Users
                     .Where(u => u.LastLoginAt != null && u.LastLoginAt >= sinceUtc)
                     .ToListAsync();
    }
}
