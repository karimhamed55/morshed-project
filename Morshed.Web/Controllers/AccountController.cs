using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Entities;
using Morshed.Web.Models;
using Morshed.Core.Interfaces;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, Microsoft.AspNetCore.Hosting.IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }

        // GET: /Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                Gender = user.Gender,
                Age = user.Age,
                FullName = $"{user.FirstName} {user.LastName}",
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                SavedPlacesCount = user.SavedPlacesCount,
                VisitedPlacesCount = user.VisitedPlacesCount,
                JoinedAt = user.CreatedAt
            };

            var bookmarks = await _unitOfWork.Bookmarks.FindAsync(b => b.UserId == user.Id, new[] { "Place", "Place.Images", "Place.Reviews" });
            model.SavedPlaces = bookmarks.Select(b => new SavedPlaceViewModel
            {
                PlaceId = b.Place.Id,
                Name = b.Place.Name,
                City = b.Place.Address, // Ideally resolve City/Province
                Rating = b.Place.Reviews.Any() ? b.Place.Reviews.Average(r => r.Rating) : 0,
                ImageUrl = b.Place.Images.FirstOrDefault()?.Url ?? "/images/default-place.jpg"
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                // Reload read-only properties
                model.Username = user.UserName;
                model.Email = user.Email;
                model.SavedPlacesCount = user.SavedPlacesCount;
                model.VisitedPlacesCount = user.VisitedPlacesCount;
                model.JoinedAt = user.CreatedAt;
                model.ProfilePictureUrl = user.ProfilePictureUrl; // Keep existing if upload failed validation
                return View(model);
            }

            // Handle Image Upload
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
                if (!System.IO.Directory.Exists(uploadsFolder))
                {
                    System.IO.Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                user.ProfilePictureUrl = "/images/profiles/" + uniqueFileName;
            }

            // Update User Details
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Country = model.Country;
            user.Gender = model.Gender;
            if (model.Age.HasValue) user.Age = model.Age.Value;
            user.PhoneNumber = model.PhoneNumber;
            user.Bio = model.Bio;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                 // Reload read-only properties
                model.Username = user.UserName;
                model.Email = user.Email;
                model.SavedPlacesCount = user.SavedPlacesCount;
                model.VisitedPlacesCount = user.VisitedPlacesCount;
                model.JoinedAt = user.CreatedAt;
                return View(model);
            }

            TempData["StatusMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        // GET: /Account/Settings
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var model = new UserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Country = user.Country,
                Gender = user.Gender,
                Age = user.Age,
                FullName = $"{user.FirstName} {user.LastName}",
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                SavedPlacesCount = user.SavedPlacesCount,
                VisitedPlacesCount = user.VisitedPlacesCount
            };

            return View(model);
        }

        // POST: /Account/Settings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Handle Image Upload
            if (model.ProfileImage != null && model.ProfileImage.Length > 0)
            {
                var uploadsFolder = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "images", "profiles");
                if (!System.IO.Directory.Exists(uploadsFolder))
                {
                    System.IO.Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfileImage.FileName;
                var filePath = System.IO.Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(fileStream);
                }

                user.ProfilePictureUrl = "/images/profiles/" + uniqueFileName;
            }

            // Update User Details
            if (model.FirstName != null) user.FirstName = model.FirstName;
            if (model.LastName != null) user.LastName = model.LastName;
            if (model.Country != null) user.Country = model.Country;
            if (model.Gender != null) user.Gender = model.Gender;
            if (model.Age.HasValue) user.Age = model.Age.Value;
            if (model.PhoneNumber != null) user.PhoneNumber = model.PhoneNumber;
            if (model.Bio != null) user.Bio = model.Bio;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["StatusMessage"] = "Settings updated successfully!";
            return RedirectToAction("Settings");
        }

    }
}
