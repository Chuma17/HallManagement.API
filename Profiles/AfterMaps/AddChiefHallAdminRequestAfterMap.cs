using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddChiefHallAdminRequestAfterMap : IMappingAction<AddChiefHallAdminRequest, ChiefHallAdmin>
    {
        public void Process(AddChiefHallAdminRequest source, ChiefHallAdmin destination, ResolutionContext context)
        {
            destination.ChiefHallAdminId = Guid.NewGuid();
        }
    }
}
