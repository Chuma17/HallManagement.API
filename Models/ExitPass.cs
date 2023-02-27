using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class ExitPass
    {
        [Key]
        public Guid ExitPassId { get; set; }
        public Guid StudentId { get; set; }
        public Guid HallId { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime DateOfExit { get; set; }
        public DateTime DateOfReturn { get; set; }
        public string? ReasonForLeaving { get; set; }
        public string? StateOfArrival { get; set; }
        public string? Address { get; set; }
        public bool HasReturned { get; set; } = false;
    }
}
