using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddHallAdminRequestAfterMap : IMappingAction<AddHallAdminRequest, HallAdmin>
    {
        public void Process(AddHallAdminRequest source, HallAdmin destination, ResolutionContext context)
        {
            destination.HallAdminId = Guid.NewGuid();
        }
    }
}
