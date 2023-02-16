using AutoMapper;
using HallManagementTest2.Models;
using HallManagementTest2.Requests.Add;

namespace HallManagementTest2.Profiles.AfterMaps
{
    public class AddComplaintFormRequestAfterMap : IMappingAction<AddComplaintFormRequest, ComplaintForm>
    {
        public void Process(AddComplaintFormRequest source, ComplaintForm destination, ResolutionContext context)
        {
            destination.ComplaintFormId = Guid.NewGuid();
        }
    }
}
