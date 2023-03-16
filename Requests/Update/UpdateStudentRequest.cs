namespace HallManagementTest2.Requests.Update
{
    public class UpdateStudentRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Gender { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Department { get; set; }
        public string? Course { get; set; }
    }
}
