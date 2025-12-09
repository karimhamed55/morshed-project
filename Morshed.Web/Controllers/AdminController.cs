using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    // [Authorize(Roles = "Admin")] 
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // === Dashboard ===
        public IActionResult Index() => RedirectToAction("Dashboard");

        public IActionResult Dashboard()
        {
            return View();
        }

        // ==========================================
        //  PLACES MANAGEMENT
        // ==========================================
        public async Task<IActionResult> ManagePlaces()
        {
            var places = await _unitOfWork.Places.GetAllAsync();
            return View(places);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePlace()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            ViewBag.Provinces = new SelectList(provinces, "Id", "NameEn");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePlace(Place place)
        {
            if (string.IsNullOrWhiteSpace(place.Name) || string.IsNullOrWhiteSpace(place.Address) || string.IsNullOrWhiteSpace(place.Category))
            {
                var provinces = await _unitOfWork.Provinces.GetAllAsync();
                ViewBag.Provinces = new SelectList(provinces, "Id", "NameEn");
                ModelState.AddModelError("", "Name, Address, and Category are required.");
                return View(place);
            }

            await _unitOfWork.Places.AddAsync(place);

            // === التصليح هنا: شلنا الكومنت عشان يحفظ في الداتابيز ===
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(ManagePlaces));
        }

        [HttpGet]
        public async Task<IActionResult> EditPlace(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null) return NotFound();

            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            ViewBag.Provinces = new SelectList(provinces, "Id", "NameEn", place.ProvinceId);

            return View(place);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPlace(Place place)
        {
            if (string.IsNullOrWhiteSpace(place.Name) || string.IsNullOrWhiteSpace(place.Address))
            {
                var provinces = await _unitOfWork.Provinces.GetAllAsync();
                ViewBag.Provinces = new SelectList(provinces, "Id", "NameEn", place.ProvinceId);
                ModelState.AddModelError("", "Name and Address are required.");
                return View(place);
            }

            var existingPlace = await _unitOfWork.Places.GetByIdAsync(place.Id);
            if (existingPlace == null) return NotFound();

            existingPlace.Name = place.Name;
            existingPlace.ProvinceId = place.ProvinceId;
            existingPlace.Address = place.Address;
            existingPlace.Description = place.Description;
            existingPlace.Category = place.Category;

            _unitOfWork.Places.Update(existingPlace);

            // === وتفعيل الحفظ هنا كمان ===
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(ManagePlaces));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place != null)
            {
                _unitOfWork.Places.Remove(place);

                // === وهنا عشان الحذف يتم ===
                await _unitOfWork.CompleteAsync();
            }
            return RedirectToAction(nameof(ManagePlaces));
        }


        // ==========================================
        //  PROVINCES MANAGEMENT
        // ==========================================
        public async Task<IActionResult> ManageProvinces()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            return View(provinces);
        }

        [HttpGet]
        public IActionResult CreateProvince()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProvince(Province province)
        {
            if (string.IsNullOrWhiteSpace(province.NameEn))
            {
                ModelState.AddModelError("NameEn", "Name is required.");
                return View(province);
            }

            var allProvinces = await _unitOfWork.Provinces.GetAllAsync();
            if (allProvinces.Any(p => p.NameEn.ToLower() == province.NameEn.ToLower()))
            {
                ModelState.AddModelError("NameEn", "Province already exists.");
                return View(province);
            }

            if (string.IsNullOrEmpty(province.NameAr)) province.NameAr = province.NameEn;
            if (string.IsNullOrEmpty(province.ThumbnailUrl)) province.ThumbnailUrl = "/images/default_province.jpg";

            await _unitOfWork.Provinces.AddAsync(province);

            // === وهنا عشان المحافظة تتحفظ ===
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(ManageProvinces));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProvince(int id)
        {
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);
            if (province != null)
            {
                _unitOfWork.Provinces.Remove(province);
                await _unitOfWork.CompleteAsync();
            }
            return RedirectToAction(nameof(ManageProvinces));
        }


        // ==========================================
        //  REVIEWS MANAGEMENT
        // ==========================================
        public async Task<IActionResult> ManageReviews()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review != null)
            {
                _unitOfWork.Reviews.Remove(review);
                await _unitOfWork.CompleteAsync();
            }
            return RedirectToAction(nameof(ManageReviews));
        }


        // ==========================================
        //  USERS MANAGEMENT
        // ==========================================
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null && await _userManager.IsInRoleAsync(user, role))
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Users));
        }
    }
}