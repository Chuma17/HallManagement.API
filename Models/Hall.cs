using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class Hall
    {
        [Key]
        public Guid HallId { get; set; }
        public int RoomCount { get; set; } = 0;
        public int AvailableRooms { get; set; } = 0;
        public int StudentCount { get; set; } = 0;
        public int RoomSpace { get; set; }
        public int BlockCount { get; set; } = 0;
        public string? HallGender { get; set; }
        public string? HallName { get; set; }
        public Guid HallTypeId { get; set; }


        public virtual ICollection<Block>? Blocks { get; set; }
        public virtual ICollection<Room>? Rooms { get; set; }
        public virtual ICollection<Porter>? Porters { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
        public virtual ICollection<ComplaintForm>? ComplaintForms { get; set; }
        public virtual ICollection<StudentDevice>? StudentDevices { get; set; }
    }
}
