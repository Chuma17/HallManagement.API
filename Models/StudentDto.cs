using HallManagementTest2.Data;

namespace HallManagementTest2.Models
{
    public class StudentDto
    {
        public Guid StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public long? Mobile { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Gender { get; set; }
        public string? State { get; set; }
        public string? Address { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public int? StudyLevel { get; set; }
        public string? School { get; set; }
        public string? Department { get; set; }
        public string? Course { get; set; }

        public Guid HallId { get; set; }
        public Guid RoomId { get; set; }

        public string Role { get; set; } = "Student";

        //public virtual Room? Room { get; set; }
        //public virtual Hall? Hall { get; set; }
        public virtual ICollection<StudentDevice>? StudentDevices { get; set; }
    }
}
