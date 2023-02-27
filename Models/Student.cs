using HallManagementTest2.Data;
using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class Student : ApplicationUser
    {
        [Key]
        public Guid StudentId { get; set; }
        public int StudyLevel { get; set; }
        public string? School { get; set; }
        public string? Department { get; set; }
        public string? Course { get; set; }
        public string MatricNo { get; set; }
        public bool IsBlocked { get; set; }

        public Guid? HallId { get; set; }
        public Guid? RoomId { get; set; }
        public Guid? BlockId { get; set; }

        public string Role { get; set; } = "Student";        
               
        public virtual ICollection<StudentDevice>? StudentDevices { get; set; }
        public virtual ICollection<ExitPass>? ExitPasses { get; set; }
    }
}
