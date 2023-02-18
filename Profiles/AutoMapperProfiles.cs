using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Profiles.AfterMaps;
using HallManagementTest2.Requests.Add;
using HallManagementTest2.Requests.Update;

namespace HallManagementTest2.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Student, StudentDto>().ReverseMap();

            CreateMap<AddStudentRequest, Student>()
                .AfterMap<AddStudentRequestAfterMap>();

            CreateMap<AddHallTypeRequest, HallType>()
                .AfterMap<AddHallTypeRequestAfterMap>();

            CreateMap<AddHallRequest, Hall>()
                .AfterMap<AddHallRequestAfterMap>();

            CreateMap<AddRoomRequest, Room>()
                .AfterMap<AddRoomRequestAfterMap>();

            CreateMap<AddStudentDeviceRequest, StudentDevice>()
                .AfterMap<AddStudentDeviceRequestAfterMap>();

            CreateMap<AddChiefHallAdminRequest, ChiefHallAdmin>()
                .AfterMap<AddChiefHallAdminRequestAfterMap>();
            
            CreateMap<AddHallAdminRequest, HallAdmin>()
                .AfterMap<AddHallAdminRequestAfterMap>();

            CreateMap<AddPorterRequest, Porter>()
                .AfterMap<AddPorterRequestAfterMap>();

            CreateMap<AddComplaintFormRequest, ComplaintForm>()
                .AfterMap<AddComplaintFormRequestAfterMap>();

            CreateMap<AddBlockRequest, Block>()
                .AfterMap<AddBlockRequestAfterMap>();

            CreateMap<AddNotificationRequest, Notification>()
                .AfterMap<AddNotificationRequesAfterMap>();


            CreateMap<UpdateStudentRequest, Student>();

            CreateMap<UpdateHallTypeRequest, HallType>();

            CreateMap<UpdateHallRequest, Hall>();

            CreateMap<UpdateStudentDeviceRequest, StudentDevice>();

            CreateMap<UpdateChiefHallAdminRequest, ChiefHallAdmin>();   
            
            CreateMap<UpdateHallAdminRequest, HallAdmin>();  
            
            CreateMap<UpdatePorterRequest, Porter>();

            CreateMap<UpdateBlockRequest, Block>();            
        }
    }
}
