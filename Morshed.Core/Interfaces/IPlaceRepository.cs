using Morshed.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Morshed.Core.Interfaces
{
    public interface IPlaceRepository : IGenericRepository<Place>
    {
        Task<IEnumerable<Place>> GetPlacesByProvinceAsync(int provinceId);
        Task<Place> GetPlaceWithDetailsAsync(int id);
    }
}
