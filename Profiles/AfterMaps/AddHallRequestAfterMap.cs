using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddHallRequestAfterMap : IMappingAction<AddHallRequest, Hall>
    {
        public void Process(AddHallRequest source, Hall destination, ResolutionContext context)
        {
            destination.HallId = Guid.NewGuid();
        }
    }
}
