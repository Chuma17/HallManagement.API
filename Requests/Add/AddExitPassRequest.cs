namespace HallManagementTest2.Requests.Add
{
    public class AddExitPassRequest
    {
        public DateTime DateOfExit { get; set; }
        public DateTime DateOfReturn { get; set; }
        public string? ReasonForLeaving { get; set; }
        public string? StateOfArrival { get; set; }
        public string? Address { get; set; }
    }
}
