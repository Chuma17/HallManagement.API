namespace HallManagementTest2.Requests.Add
{
    public class AddComplaintFormRequest
    {
        public string? RoomNumber { get; set; }
        public string? Plumbing { get; set; }
        public string? Carpentary { get; set; }
        public string? Electrical { get; set; }
        public string? Others { get; set; }

        public Guid HallId { get; set; }
    }
}
