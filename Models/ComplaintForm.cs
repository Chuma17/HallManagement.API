namespace HallManagementTest2.Models
{
    public class ComplaintForm
    {
        public Guid ComplaintFormId { get; set; }
        public string? RoomNumber { get; set; }
        public string? Plumbing { get; set; }
        public string? Carpentary { get; set; }
        public string? Electrical { get; set; }
        public string? Others { get; set; }


        public Guid HallId { get; set; }
        public Guid BlockId { get; set; }
        public Guid RoomId { get; set; }
    }
}
