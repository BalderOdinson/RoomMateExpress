using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using RoomMateExpressWebApi.EF.Models;

namespace RoomMateExpressWebApi.EF.Services
{
    public class NeighborhoodService : INeighborhoodService
    {
        private readonly RoomMateExpressDbContext _context;

        public NeighborhoodService(RoomMateExpressDbContext context)
        {
            _context = context;
        }

        public async Task<Neighborhood> CreateNeighborhood(Neighborhood neighborhood)
        {

            var old = await _context.Neighborhoods.FirstOrDefaultAsync(a => a.Id == neighborhood.Id && a.Name == neighborhood.Name);
            if (old == null)
            {
                _context.Neighborhoods.Add(neighborhood);
                await _context.SaveChangesAsync();
            }
            return neighborhood;
        }

        public async Task<Neighborhood> UpdateNeighborhood(Neighborhood neighborhood, Guid id)
        {
            var neighborhoodOld = await _context.Neighborhoods.FirstOrDefaultAsync((c => c.Id == neighborhood.Id));
            if (neighborhoodOld == null)
            {
                return null;
            }
            _context.Entry(neighborhoodOld).CurrentValues.SetValues(neighborhood);
            await _context.SaveChangesAsync();
            return neighborhood;
        }

        public async Task<bool> DeleteNeighborhood(Guid id)
        {
            var neighborhood = await _context.Neighborhoods.FirstOrDefaultAsync(n => n.Id == id);
            if (neighborhood == null)
                return false;
            _context.Neighborhoods.Remove(neighborhood);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Neighborhood>> GetAllNeighborhoods()
        {
            return await _context.Neighborhoods.ToListAsync();
        }

        public async Task<Neighborhood> GetNeighborhood(Guid id)
        {
            return await _context.Neighborhoods.FirstOrDefaultAsync(n => n.Id.Equals(id));
        }
    }
}
