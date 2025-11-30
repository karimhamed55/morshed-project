using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Interfaces;
using Morshed.Core.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Morshed.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return View(Enumerable.Empty<Place>());
            }

            // التعديل هنا: زودنا p.Address.Contains(query)
            var places = await _unitOfWork.Places.FindAsync(p =>
                p.Name.Contains(query) ||
                p.Description.Contains(query) ||
                p.Category.Contains(query) ||
                p.Address.Contains(query) // <--- ده السطر اللي هيخلي Luxor تظهر
            );

            ViewBag.Query = query;

            return View(places);
        }
    }
}