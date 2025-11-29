using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Entities;
using Morshed.Core.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlaceController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IActionResult> Details(int id)
        {
            var place = await _unitOfWork.Places.GetPlaceWithDetailsAsync(id);
            if (place == null) return NotFound();
            return View(place);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int placeId, int rating, string comment)
        {
            var userId = _userManager.GetUserId(User);
            var review = new Review
            {
                PlaceId = placeId,
                UserId = userId,
                Rating = rating,
                Comment = comment
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Details), new { id = placeId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Bookmark(int placeId)
        {
            var userId = _userManager.GetUserId(User);
            
            // Check if already bookmarked
            var existing = await _unitOfWork.Bookmarks.FindAsync(b => b.UserId == userId && b.PlaceId == placeId);
            if (!existing.Any())
            {
                var bookmark = new Bookmark
                {
                    UserId = userId,
                    PlaceId = placeId
                };
                await _unitOfWork.Bookmarks.AddAsync(bookmark);
                await _unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Details), new { id = placeId });
        }
    }
}
