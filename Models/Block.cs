namespace HallManagementTest2.Models
{
    public class Block
    {
        public Guid BlockId { get; set; }
        public string? BlockName { get; set; }
        public int RoomCount { get; set; } = 0;
        public int AvailableRooms { get; set; } = 0;
        public int StudentCount { get; set; } = 0;
        public int RoomSpace { get; set; }
        public string? BlockGender { get; set; }
        public Guid HallId { get; set; }

        public virtual ICollection<Room>? Rooms { get; set; }
        public virtual ICollection<Student>? Students { get; set; }
    }
}
