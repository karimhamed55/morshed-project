using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    [Authorize]
    public class TourController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TourController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tours = await _unitOfWork.Tours.GetUserToursAsync(userId);
            return View(tours);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Provinces = new SelectList(await _unitOfWork.Provinces.GetAllAsync(), "Id", "NameEn");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tour tour)
        {
            if (ModelState.IsValid)
            {
                tour.UserId = _userManager.GetUserId(User);
                await _unitOfWork.Tours.AddAsync(tour);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Provinces = new SelectList(await _unitOfWork.Provinces.GetAllAsync(), "Id", "NameEn");
            return View(tour);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tour = await _unitOfWork.Tours.GetTourWithDetailsAsync(id);
            if (tour == null) return NotFound();
            return View(tour);
        }
    }
}
