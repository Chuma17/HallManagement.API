using HallManagementTest2.Data;
using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class Porter : ApplicationUser
    {
        [Key]
        public Guid PorterId { get; set; } 
        public string Role { get; set; } = "Porter";

        public Guid HallId { get; set; }
    }
}
