using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(Enumerable.Empty<Morshed.Core.Entities.Place>());
            }

            var places = await _unitOfWork.Places.FindAsync(p => 
                p.Name.Contains(query) || 
                p.Description.Contains(query) ||
                p.Category.Contains(query));

            ViewBag.Query = query;
            return View(places);
        }
    }
}
