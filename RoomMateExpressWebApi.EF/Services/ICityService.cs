using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public interface ICityService
    {
        Task<List<City>> GetAllCities();

        Task<City> GetCity(Guid id);

        Task<City> CreateCity(City city);

        Task<City> UpdateCity(City city, Guid id);

        Task<bool> DeleteCity(Guid id);
    }
}
