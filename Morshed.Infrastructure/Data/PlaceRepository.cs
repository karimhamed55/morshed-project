using Microsoft.EntityFrameworkCore;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Infrastructure.Data
{
    public class PlaceRepository : GenericRepository<Place>, IPlaceRepository
    {
        public PlaceRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Place>> GetPlacesByProvinceAsync(int provinceId)
        {
            return await _context.Places
                .Where(p => p.ProvinceId == provinceId)
                .Include(p => p.Images)
                .ToListAsync();
        }

        public async Task<Place> GetPlaceWithDetailsAsync(int id)
        {
            return await _context.Places
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .ThenInclude(r => r.User)
                .Include(p => p.Province)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
