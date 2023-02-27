using HallManagementTest2.Data;
using System.ComponentModel.DataAnnotations;

namespace HallManagementTest2.Models
{
    public class StudentDevice
    {
        [Key]
        public Guid StudentDeviceId { get; set; }
        public Guid HallId { get; set; }
        public Guid? StudentId { get; set; }
        public string? MatricNo { get; set; }
        public string? Item { get; set; }
        public string? Color { get; set; }
        public string? Description { get; set; }
        public string? SerialNo { get; set; }
    }
}
