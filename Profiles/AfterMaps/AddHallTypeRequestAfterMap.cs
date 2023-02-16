using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddHallTypeRequestAfterMap : IMappingAction<AddHallTypeRequest, HallType>
    {
        public void Process(AddHallTypeRequest source, HallType destination, ResolutionContext context)
        {
            destination.HallTypeId = Guid.NewGuid();
        }
    }
}
