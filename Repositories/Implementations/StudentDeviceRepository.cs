using HallManagementTest2.Data;
using HallManagementTest2.Models;
using HallManagementTest2.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HallManagementTest2.Repositories.Implementations
{
    public class StudentDeviceRepository : IStudentDeviceRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHallRepository _hallRepository;
        private readonly IStudentRepository _studentRepository;

        public StudentDeviceRepository(ApplicationDbContext context, IHallRepository hallRepository
                                        ,IStudentRepository studentRepository)
        {
            _context = context;
            _hallRepository = hallRepository;
            _studentRepository = studentRepository;
        }
        public async Task<StudentDevice> AddStudentDeviceAsync(StudentDevice request)
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
            return await _context.StudentDevices.Include(s => s.Student).FirstOrDefaultAsync(x => x.StudentDeviceId == studentDeviceId);
        }        

        public async Task<List<StudentDevice>> GetStudentsByMatricNo(Guid hallId, string matricNo)
        {
            var existingHall = await _hallRepository.GetHallAsync(hallId);
            var Students = await _studentRepository.GetStudentsAsync();
            if (existingHall != null)
            {
                foreach (var student in Students)
                {
                    if (student.MatricNo == matricNo)
                    {
                        return student.StudentDevices.ToList();
                    }
                }
            }
            return null;
        }

        public async Task<StudentDevice> UpdateStudentDevice(Guid studentDeviceId, StudentDevice request)
        {
            var existingstudentDevice = await GetStudentDeviceAsync(studentDeviceId);
            if (existingstudentDevice != null)
            {
                existingstudentDevice.SerialNo = request.SerialNo;
                existingstudentDevice.Item = request.Item;
                existingstudentDevice.Color = request.Color;
                existingstudentDevice.Description = request.Description;

                await _context.SaveChangesAsync();
                return existingstudentDevice;
            }
            return null;
        }
    }
}
