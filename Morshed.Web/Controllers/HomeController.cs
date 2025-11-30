using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Interfaces;
using Morshed.Web.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            return View(provinces);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}