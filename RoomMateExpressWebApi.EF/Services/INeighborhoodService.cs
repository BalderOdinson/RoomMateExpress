using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface INeighborhoodService 
    {

        Task<List<Neighborhood>> GetAllNeighborhoods();

        Task<Neighborhood> GetNeighborhood(Guid id);

        Task<Neighborhood> CreateNeighborhood(Neighborhood neighborhood);

        Task<Neighborhood> UpdateNeighborhood(Neighborhood neighborhood, Guid id);

        Task<bool> DeleteNeighborhood(Guid id);
    }
}
