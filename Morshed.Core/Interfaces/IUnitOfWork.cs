using System;
using System.Threading.Tasks;
using Morshed.Core.Entities;

namespace Morshed.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPlaceRepository Places { get; }
        ITourRepository Tours { get; }
        IGenericRepository<Province> Provinces { get; }
        IGenericRepository<Review> Reviews { get; }
        IGenericRepository<Bookmark> Bookmarks { get; }
        
        Task<int> CompleteAsync();
    }
}
