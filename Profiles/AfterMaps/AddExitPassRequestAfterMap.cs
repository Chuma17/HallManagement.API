using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddExitPassRequestAfterMap : IMappingAction<AddExitPassRequest, ExitPass>
    {
        public void Process(AddExitPassRequest source, ExitPass destination, ResolutionContext context)
        {
            destination.ExitPassId = Guid.NewGuid();
        }
    }
}
