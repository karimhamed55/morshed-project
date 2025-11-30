using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Morshed.Core.Entities;
using Morshed.Web.Models;
using System.Threading.Tasks;

namespace Morshed.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ================= REGISTER =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create user with Username from form
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Country = model.Country,
                    Gender = model.Gender
                    // Age, Bio, ProfilePictureUrl set by constructor defaults
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        // ================= LOGIN =================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 1. Find user by Email first
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    // 2. Sign in using the found Username
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        if (await _userManager.IsInRoleAsync(user, Morshed.Core.Constants.Roles.Admin))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // ================= LOGOUT =================
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ================= FORGOT PASSWORD (NEW) =================
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user exists (Optional: don't reveal this to user for security)
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null) // && await _userManager.IsEmailConfirmedAsync(user)
                {
                    // Generate Token & Send Email Logic here...
                    // var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                }

                // Redirect to confirmation page regardless (Security Best Practice)
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
    }
}