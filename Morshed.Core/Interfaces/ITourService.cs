using Morshed.Core.Entities;
using System.Threading.Tasks;

namespace Morshed.Core.Interfaces
{
    public interface ITourService
    {
        Task<decimal> CalculateTourCostAsync(int tourId);
        Task GenerateItineraryAsync(int tourId);
    }
}
