using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddBlockRequestAfterMap : IMappingAction<AddBlockRequest, Block>
    {
        public void Process(AddBlockRequest source, Block destination, ResolutionContext context)
        {
            destination.BlockId = Guid.NewGuid();
        }
    }
}
