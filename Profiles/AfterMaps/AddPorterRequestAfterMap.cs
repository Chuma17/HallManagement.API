using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddPorterRequestAfterMap : IMappingAction<AddPorterRequest, Porter>
    {
        public void Process(AddPorterRequest source, Porter destination, ResolutionContext context)
        {
            destination.PorterId = Guid.NewGuid();
        }
    }
}
