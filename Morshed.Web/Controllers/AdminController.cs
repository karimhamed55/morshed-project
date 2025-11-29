using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using Morshed.Core.Constants;
using System.Threading.Tasks;
using System.Linq;

namespace Morshed.Web.Controllers
{
    [Authorize(Roles = Roles.Admin)] 
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

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> ManagePlaces()
        {
            var places = await _unitOfWork.Places.GetAllAsync();
            return View(places);
        }

        public async Task<IActionResult> ManageReviews()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();
            return View(reviews);
        }

        // Add Place
        [HttpGet]
        public async Task<IActionResult> CreatePlace()
        {
            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            ViewBag.Provinces = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(provinces, "Id", "NameEn");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlace(Place place)
        {
            if (string.IsNullOrWhiteSpace(place.Name) || string.IsNullOrWhiteSpace(place.Address) || string.IsNullOrWhiteSpace(place.Category))
            {
                var provinces = await _unitOfWork.Provinces.GetAllAsync();
                ViewBag.Provinces = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(provinces, "Id", "NameEn");
                ModelState.AddModelError("", "Name, Address, and Category are required.");
                return View(place);
            }
            await _unitOfWork.Places.AddAsync(place);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("ManagePlaces");
        }

        // Edit Place
        [HttpGet]
        public async Task<IActionResult> EditPlace(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null) return NotFound();

            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            ViewBag.Provinces = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(provinces, "Id", "NameEn", place.ProvinceId);
            
            return View(place);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlace(Place place)
        {
            if (string.IsNullOrWhiteSpace(place.Name) || string.IsNullOrWhiteSpace(place.Address))
            {
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
            // Note: Images update logic would go here if implemented

            _unitOfWork.Places.Update(existingPlace);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("ManagePlaces");
        }

        // Delete Place
        [HttpPost]
        public async Task<IActionResult> DeletePlace(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null) return NotFound();
            _unitOfWork.Places.Remove(place);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction("ManagePlaces");
        }

        // List users and assign roles
        [HttpGet]
        public IActionResult ManageRoles()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // Assign role to user
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
            await _userManager.AddToRoleAsync(user, role);
            return RedirectToAction("ManageRoles");
        }
        // Create Province
        [HttpGet]
        public IActionResult CreateProvince()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProvince(Province province)
        {
            if (string.IsNullOrWhiteSpace(province.NameEn))
            {
                ModelState.AddModelError("NameEn", "Name is required.");
                return View(province);
            }

            var provinces = await _unitOfWork.Provinces.GetAllAsync();
            if (provinces.Any(p => p.NameEn.Equals(province.NameEn, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("NameEn", "Province already exists.");
                return View(province);
            }

            province.NameAr = province.NameEn; // Fallback
            province.Description = "Added via Admin";
            province.ThumbnailUrl = "/images/placeholder.jpg"; // Placeholder

            await _unitOfWork.Provinces.AddAsync(province);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction("CreatePlace");
        }
    }
}
