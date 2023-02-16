using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class HallType
    {
        [Key]
        public Guid HallTypeId { get; set; }
        public string? Description { get; set; }
        public int HallCount { get; set; }
        public int RoomSpaceCount { get; set; }

        public IEnumerable<Hall>? Halls { get; set; }
    }
}
