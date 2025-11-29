using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Threading.Tasks;

namespace Morshed.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Places = new PlaceRepository(_context);
            Tours = new TourRepository(_context);
            Provinces = new GenericRepository<Province>(_context);
            Reviews = new GenericRepository<Review>(_context);
            Bookmarks = new GenericRepository<Bookmark>(_context);
        }

        public IPlaceRepository Places { get; private set; }
        public ITourRepository Tours { get; private set; }
        public IGenericRepository<Province> Provinces { get; private set; }
        public IGenericRepository<Review> Reviews { get; private set; }
        public IGenericRepository<Bookmark> Bookmarks { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
