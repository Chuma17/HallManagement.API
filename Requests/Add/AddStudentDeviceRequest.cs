﻿namespace HallManagementTest2.Requests.Add
{
    public class AddStudentDeviceRequest
    {
        public string? MatricNo { get; set; }
        public string? Item { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? SerialNo { get; set; }
        public string? DeviceGender { get; set; }

        public Guid HallId { get; set; }
        public Guid StudentId { get; set; }

    }
}
