using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Interfaces;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    public class ProvinceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProvinceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            return View(provinces);
        }

        public async Task<IActionResult> Details(int id)
        {
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);
            if (province == null) return NotFound();

            var places = await _unitOfWork.Places.GetPlacesByProvinceAsync(id);
            province.Places = places.ToList();

            return View(province);
        }
    }
}
