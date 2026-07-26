using System;

namespace ProductionBoard.Core.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public bool IsActive { get; set; }

        public bool MustChangePassword { get; set; }

        public int FailedLoginCount { get; set; }

        public DateTime? LockedUntilUtc { get; set; }

        public DateTime? LastLoginAtUtc { get; set; }
    }
}