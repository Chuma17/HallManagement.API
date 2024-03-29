﻿using HallManagementTest2.Models;

namespace HallManagementTest2.Repositories.Interfaces
{
    public interface IHallTypeRepository
    {
        Task<List<HallType>> GetHallTypesAsync();
        Task<HallType> AddHallTypeAsync(HallType request);
        Task<bool> Exists(Guid hallTypeId);
        Task<HallType> DeleteHallTypeAsync(Guid hallTypeId);
        Task<HallType> GetHallTypeAsync(Guid hallTypeId);
        Task<HallType> GetHallsInHallType(Guid hallTypeId);
        Task<HallType> GetHallTypeForHall(Guid hallTypeId);

    }
}
