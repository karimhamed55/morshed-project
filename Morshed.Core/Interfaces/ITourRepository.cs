using Morshed.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Morshed.Core.Interfaces
{
    public interface ITourRepository : IGenericRepository<Tour>
    {
        Task<IEnumerable<Tour>> GetUserToursAsync(string userId);
        Task<Tour> GetTourWithDetailsAsync(int id);
    }
}
