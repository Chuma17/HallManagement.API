using HallManagementTest2.Data;
using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class Room
    {
        [Key]
        public Guid RoomId { get; set; }
        public Guid HallId { get; set; }
        public Guid BlockId { get; set; }
        public string? RoomNumber { get; set; }
        public int MaxOccupants { get; set; }
        public int AvailableSpace { get; set; }
        public int StudentCount { get; set; } = 0;
        public string? RoomGender { get; set; }
        public bool IsUnderMaintenance { get; set; } = false;
        public bool IsFull { get; set; } = false;
        
        public virtual ICollection<Student>? Students { get; set; }
    }
}
