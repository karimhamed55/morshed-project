using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using Morshed.Core.Constants; // لو عندك كلاس للثوابت
using System.Linq;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    // [Authorize(Roles = "Admin")] // فعل السطر ده لما تخلص عشان تحمي لوحة التحكم
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
            if (string.IsNullOrWhiteSpace(place.Name) || string.IsNullOrWhiteSpace(place.Address))
            {
                var provinces = await _unitOfWork.Provinces.GetAllAsync();
                ViewBag.Provinces = new SelectList(provinces, "Id", "NameEn");
                ModelState.AddModelError("", "Name and Address are required.");
                return View(place);
            }

            await _unitOfWork.Places.AddAsync(place);
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
            var existingPlace = await _unitOfWork.Places.GetByIdAsync(place.Id);
            if (existingPlace == null) return NotFound();

            existingPlace.Name = place.Name;
            existingPlace.ProvinceId = place.ProvinceId;
            existingPlace.Address = place.Address;
            existingPlace.Description = place.Description;
            existingPlace.Category = place.Category;

            _unitOfWork.Places.Update(existingPlace);
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

            await _unitOfWork.Provinces.AddAsync(province);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(ManageProvinces));
        }

        [HttpGet]
        public async Task<IActionResult> EditProvince(int id)
        {
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);
            if (province == null) return NotFound();
            return View(province);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProvince(Province province)
        {
            var existingProvince = await _unitOfWork.Provinces.GetByIdAsync(province.Id);
            if (existingProvince == null) return NotFound();

            existingProvince.NameEn = province.NameEn;
            existingProvince.NameAr = province.NameAr;
            existingProvince.Description = province.Description;

            _unitOfWork.Provinces.Update(existingProvince);
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
        //  REVIEWS MANAGEMENT (الأكشن اللي كان ناقصك)
        // ==========================================
        public async Task<IActionResult> ManageReviews()
        {
            // هات كل المراجعات
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
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
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