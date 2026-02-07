using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Infrastructure.Services
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<decimal> CalculateTourCostAsync(int tourId)
        {
            var tour = await _unitOfWork.Tours.GetTourWithDetailsAsync(tourId);
            if (tour == null) return 0;

            decimal totalCost = 0;
            foreach (var stop in tour.Stops)
            {
                // totalCost += stop.Place.AvgCost; // AvgCost removed
            }

            // Add transport costs if implemented logic exists
            // For now, just sum place costs
            return totalCost;
        }

        public async Task GenerateItineraryAsync(int tourId)
        {
            // Logic to auto-arrange stops or suggest itinerary
            // For now, simple implementation or placeholder
            await Task.CompletedTask;
        }
    }
}
