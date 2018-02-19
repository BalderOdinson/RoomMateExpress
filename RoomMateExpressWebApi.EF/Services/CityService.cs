using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class CityService : ICityService
    {
        private readonly RoomMateExpressDbContext _context;

        public CityService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<List<City>> GetAllCities()
        {
            var cities = await _context.Cities.Include(a => a.Neighborhoods).OrderBy(c => c.Name).ToListAsync();
            foreach (var city in cities)
            {
                city.Neighborhoods.Sort((neighborhood, neighborhood1) => neighborhood.Name.CompareTo(neighborhood1.Name));
            }

            return cities;
        }

        public async Task<City> GetCity(Guid id)
        {
            return await _context.Cities.Include(a => a.Neighborhoods).FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        public async Task<City> CreateCity(City city)
        {
            
            var old = await _context.Cities.FirstOrDefaultAsync(a => a.Id == city.Id && a.Name==city.Name);
            if (old == null)
            {
                _context.Cities.Add(city);
                await _context.SaveChangesAsync();
            }
            return city;
        }


        public async Task<City> UpdateCity(City city, Guid id)
        {
            var cityOld = await _context.Cities.FirstOrDefaultAsync((c => c.Id == city.Id));
            if (cityOld == null)
            {
                return null;
            }
            _context.Entry(cityOld).CurrentValues.SetValues(city);
            await _context.SaveChangesAsync();
            return city;
        }

        public async Task<bool> DeleteCity(Guid id)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(a => a.Id == id);
            if (city == null)
                return false;
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return true;
        }

    }

}
