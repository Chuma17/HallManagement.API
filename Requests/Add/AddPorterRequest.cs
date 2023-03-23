namespace HallManagementTest2.Requests.Add
{
    public class AddPorterRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public string UserName { get; set; } = string.Empty;

        public Guid HallId { get; set; }
    }
}
