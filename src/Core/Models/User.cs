using System;

namespace OTUserManagementSystem.src.Core.Models
{
    public class User
    {
        public Guid Id
        {
            get;
            set;
        } = Guid.NewGuid();

        public string Username
        {
            get;
            set;
        } = null!;

        public string Email
        {
            get;
            set;
        } = null!;

        public string PasswordHash
        {
            get;
            set;
        } = null!;

        public string? FirstName
        {
            get;
            set;
        }

        public string? LastName
        {
            get;
            set;
        }

        public DateTime CreatedAt
        {
            get;
            set;
        } = DateTime.Now;

        public DateTime? LastLoginAt
        {
            get;
            set;
        }
    }
}
