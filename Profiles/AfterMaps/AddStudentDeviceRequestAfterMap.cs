using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddStudentDeviceRequestAfterMap : IMappingAction<AddStudentDeviceRequest, StudentDevice>
    {
        public void Process(AddStudentDeviceRequest source, StudentDevice destination, ResolutionContext context)
        {
            destination.StudentDeviceId = Guid.NewGuid();
        }
    }
}
