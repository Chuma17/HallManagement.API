﻿namespace HallManagementTest2.Data
{
    public class ApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string Gender { get; set; }        
        public string UserName { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; } = DateTime.MinValue;
        public DateTime TokenExpires { get; set; } = DateTime.MinValue;
    }
}
