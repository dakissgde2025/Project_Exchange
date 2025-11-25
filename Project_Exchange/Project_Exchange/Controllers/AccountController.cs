using System.Security.Claims;
using BusinessLogic.Entities;
using BusinessLogic.Managers.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Exchange.Models;

namespace Project_Exchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppUserManager _appUserManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAppUserManager appUserManager, ILogger<AccountController> logger)
        {
            _appUserManager = appUserManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (await _appUserManager.EmailExistsAsync(model.Email, cancellationToken))
            {
                ModelState.AddModelError(nameof(RegisterViewModel.Email), "Ezzel az email címmel már létezik felhasználó.");
                return View(model);
            }

            try
            {
                var user = await _appUserManager.CreateAsync(model.Email, model.Password, model.PhoneNumber, cancellationToken);
                await SignInAsync(user, rememberMe: true);
                TempData["FlashMessage"] = "Sikeres regisztráció! A foglalási felületre irányítottuk.";
                return RedirectToAction("Index", "Appointments");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nem sikerült létrehozni a felhasználót.");
                ModelState.AddModelError(string.Empty, "Váratlan hiba történt. Próbálja meg később.");
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _appUserManager.ValidateCredentialsAsync(model.Email, model.Password, cancellationToken);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Hibás email vagy jelszó.");
                return View(model);
            }

            await SignInAsync(user, model.RememberMe);
            TempData["FlashMessage"] = "Sikeres bejelentkezés!";

            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Appointments");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["FlashMessage"] = "Sikeresen kijelentkeztél.";
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(AppUser user, bool rememberMe)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = rememberMe,
                    AllowRefresh = true
                });
        }
    }
}

