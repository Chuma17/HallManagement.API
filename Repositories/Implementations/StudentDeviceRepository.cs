using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace HallManagementTest2.Repositories.Implementations
{
    public class StudentDeviceRepository : IStudentDeviceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHallRepository _hallRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IStudentDeviceRepository _studentDeviceRepository;

        public StudentDeviceRepository(ApplicationDbContext context, IHallRepository hallRepository
                                        ,IStudentRepository studentRepository)
        {
            _context = context;
            _hallRepository = hallRepository;
            _studentRepository = studentRepository;
        }
        public async Task<StudentDevice> AddStudentDeviceAsync(Guid hallId, StudentDevice request)
        {
            var studentDevice = await _context.StudentDevices.AddAsync(request);
            await _context.SaveChangesAsync();
            return studentDevice.Entity;
        }

        public async Task<StudentDevice> DeleteStudentDeviceAsync(Guid studentDeviceId)
        {
            var studentDevice = await GetStudentDeviceAsync(studentDeviceId);

            if (studentDevice != null)
            {
                _context.StudentDevices.Remove(studentDevice);
                await _context.SaveChangesAsync();
                return studentDevice;
            }

            return null;
        }

        public async Task<bool> Exists(Guid studentDeviceId)
        {
            return await _context.StudentDevices.AnyAsync(x => x.StudentDeviceId == studentDeviceId);
        }

        public async Task<StudentDevice> GetStudentDeviceAsync(Guid studentDeviceId)
        {
            return await _context.StudentDevices.FirstOrDefaultAsync(x => x.StudentDeviceId == studentDeviceId);
        }

        public async Task<List<StudentDevice>> GetStudentDevices()
        {
            var devices = _context.StudentDevices.ToListAsync();
            return await devices;
        }

        public async Task<List<StudentDevice>> GetStudentDevicesInHall(Guid hallId)
        {
            var devices = await GetStudentDevices();
            var devicesInHall = new List<StudentDevice>();
            foreach (var device in devices)
            {
                if (device.HallId == hallId)
                {
                    devicesInHall.Add(device);
                }
            }
            return devicesInHall;
        }

        public async Task<List<StudentDevice>> GetStudentDevicesByMatricNo(string matricNo)
        {
            var devices = await GetStudentDevices();
            var studentDevices = new List<StudentDevice>();

            foreach (var device in devices)
            {
                if (device.MatricNo == matricNo)
                {
                    studentDevices.Add(device);
                }
            }
            return studentDevices;            
        }

        public async Task<StudentDevice> UpdateStudentDevice(Guid studentDeviceId, StudentDevice request)
        {
            var existingstudentDevice = await GetStudentDeviceAsync(studentDeviceId);
            if (existingstudentDevice != null)
            {
                existingstudentDevice.HallId = request.HallId;
                existingstudentDevice.StudentId = request.StudentId;
                existingstudentDevice.MatricNo = request.MatricNo;

                await _context.SaveChangesAsync();
                return existingstudentDevice;
            }
            return null;
        }

        public async Task<List<StudentDevice>> GetStudentDevicesForStudent(Guid studentId)
        {
            var devices = await GetStudentDevices();
            var devicesInHall = new List<StudentDevice>();
            foreach (var device in devices)
            {
                if (device.StudentId == studentId)
                {
                    devicesInHall.Add(device);
                }
            }
            return devicesInHall;
        }
    }
}
