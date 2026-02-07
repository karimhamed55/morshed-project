using Microsoft.EntityFrameworkCore;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Infrastructure.Data
{
    public class TourRepository : GenericRepository<Tour>, ITourRepository
    {
        public TourRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Tour> GetTourWithDetailsAsync(int id)
        {
            return await _context.Tours
                .Include(t => t.Stops)
                .ThenInclude(ts => ts.Place)
                .Include(t => t.Province)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tour>> GetUserToursAsync(string userId)
        {
            return await _context.Tours
                .Where(t => t.UserId == userId)
                .Include(t => t.Province)
                .ToListAsync();
        }
    }
}
