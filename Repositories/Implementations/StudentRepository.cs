using HallManagementTest2.Data;
using HallManagementTest2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using HallManagementTest2.Repositories.Interfaces;

namespace HallManagementTest2.Repositories.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Student> AddStudentAsync(Student request)
        {
            var student = await _context.Students.AddAsync(request);
            await _context.SaveChangesAsync();
            return student.Entity;
        }

        public async Task<Student> DeleteStudentAsync(Guid studentId)
        {
            var student = await GetStudentAsync(studentId);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return student;
            }

            return null;
        }

        public async Task<bool> Exists(Guid studentId)
        {
            return await _context.Students.AnyAsync(x => x.StudentId == studentId);
        }

        public async Task<Student> GetStudentAsync(Guid studentId)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == studentId);
            return student;
        }

        public async Task<Student> GetStudentByMatricNo(string matricNo)
        {
            var student = await _context.Students.FirstOrDefaultAsync(x => x.MatricNo == matricNo);
            if (student != null)
            {
                return student;
            }
            return null;
        }

        public async Task<Student> GetStudentDevicesAsync(Guid studentId)
        {
            var studentDevices = await _context.Students.Include(s => s.StudentDevices).FirstOrDefaultAsync(x => x.StudentId == studentId);
            return studentDevices;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            var students = await _context.Students.ToListAsync();

            return students;
        }

        public async Task<List<Student>> GetStudentsInHall(Guid hallId)
        {
            var students = await GetStudentsAsync();
            var studentsInHall = new List<Student>();
            foreach (var student in students)
            {
                if (student.HallId == hallId)
                {
                    studentsInHall.Add(student);
                }
            }
            return studentsInHall;
        }

        public async Task<Student> JoinBlock(Guid? blockId, Guid studentId)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.BlockId = blockId;

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> JoinHall(Guid? hallId, Guid studentId)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.HallId = hallId;

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;            
        }

        public async Task<Student> JoinRoom(Guid? roomId, Guid studentId)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.RoomId = roomId;                

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }
        
        public async Task<bool> UpdateProfileImage(Guid studentId, string profileImageUrl)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> UpdateStudent(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {
                existingStudent.FirstName = request.FirstName;
                existingStudent.LastName = request.LastName;
                existingStudent.Email = request.Email;
                existingStudent.DateOfBirth = request.DateOfBirth;
                existingStudent.Mobile = request.Mobile;
                existingStudent.Gender = request.Gender;
                existingStudent.Address = request.Address;
                existingStudent.School = request.School;
                existingStudent.Course = request.Course;
                existingStudent.State = request.State;
                existingStudent.Department = request.Department;                
                existingStudent.UserName = request.UserName;
                existingStudent.StudyLevel = request.StudyLevel;
                existingStudent.HallId = request.HallId;
                existingStudent.RoomId = request.RoomId;

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> UpdateStudentToken(string matricNo, Student request)
        {
            var existingStudent = await GetStudentByMatricNo(matricNo);
            if (existingStudent != null)
            {
                existingStudent.AccessToken = request.AccessToken;
                existingStudent.RefreshToken = request.RefreshToken;

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }

        public async Task<Student> UpdateStudentPasswordHash(Guid studentId, Student request)
        {
            var existingStudent = await GetStudentAsync(studentId);
            if (existingStudent != null)
            {                
                existingStudent.PasswordHash = request.PasswordHash;
                existingStudent.PasswordSalt = request.PasswordSalt;               

                await _context.SaveChangesAsync();
                return existingStudent;
            }
            return null;
        }
    }
}