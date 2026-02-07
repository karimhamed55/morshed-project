using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Interfaces;
using System.Linq;
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

        // 1. صفحة عرض كل المحافظات
        public async Task<IActionResult> Index()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            return View(provinces);
        }

        // 2. صفحة تفاصيل محافظة واحدة
        public async Task<IActionResult> Details(int id)
        {
            // بنجيب المحافظة
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);

            if (province == null) return NotFound();

            // بنجيب الأماكن المرتبطة بيها ونحطها جواها
            var places = await _unitOfWork.Places.FindAsync(p => p.ProvinceId == id);
            province.Places = places.ToList();

            return View(province);
        }
    }
}