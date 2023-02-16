using HallManagementTest2.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class ChiefHallAdmin : ApplicationUser
    {
        [Key] 
        public Guid ChiefHallAdminId { get; set; }
        public string Role { get; set; } = "ChiefHallAdmin";

    }
}
