using Microsoft.EntityFrameworkCore;
using OTUserManagementSystem.src.Core.Models;

namespace OTUserManagementSystem.src.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                   .HasIndex(u => u.Username)
                   .IsUnique();

            builder.Entity<User>()
                   .HasIndex(u => u.Email)
                   .IsUnique();
        }
    }
}
