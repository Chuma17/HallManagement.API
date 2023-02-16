namespace HallManagementTest2.Requests.Add
{
    public class AddHallAdminRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public long? Mobile { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Gender { get; set; }
        public string? State { get; set; }
        public string? Address { get; set; }
        public string UserName { get; set; } = string.Empty;
        public Guid HallId { get; set; }
    }
}
