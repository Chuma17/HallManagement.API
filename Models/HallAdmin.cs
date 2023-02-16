using HallManagementTest2.Data;
using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class HallAdmin : ApplicationUser
    {
        [Key]
        public Guid HallAdminId { get; set; }
        public string Role { get; set; } = "HallAdmin";

        public Guid HallId { get; set; }
    }
}
