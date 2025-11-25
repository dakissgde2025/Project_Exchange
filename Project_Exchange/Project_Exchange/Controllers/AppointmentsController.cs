using System.Security.Claims;
using BusinessLogic.Managers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Exchange.Models;

namespace Project_Exchange.Controllers
{
    [Authorize]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentManager _appointmentManager;
        private readonly ILogger<AppointmentsController> _logger;

        public AppointmentsController(IAppointmentManager appointmentManager, ILogger<AppointmentsController> logger)
        {
            _appointmentManager = appointmentManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken) =>
            View(await BuildPageViewModelAsync(null, cancellationToken));

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentPageViewModel model, CancellationToken cancellationToken)
        {
            if (model.Form == null)
            {
                ModelState.AddModelError(string.Empty, "Az űrlap adatai hiányoznak.");
                return View("Index", await BuildPageViewModelAsync(new AppointmentFormViewModel(), cancellationToken));
            }

            if (!ModelState.IsValid || model.Form.AppointmentDateTime == null)
            {
                return View("Index", await BuildPageViewModelAsync(model.Form, cancellationToken));
            }

            var desiredSlot = DateTime.SpecifyKind(model.Form.AppointmentDateTime.Value, DateTimeKind.Local);
            if (desiredSlot <= DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "Csak jövőbeni időpont foglalható.");
                return View("Index", await BuildPageViewModelAsync(model.Form, cancellationToken));
            }

            try
            {
                await _appointmentManager.CreateAsync(
                    GetCurrentUserId(),
                    desiredSlot,
                    model.Form.Note,
                    cancellationToken);

                TempData["FlashMessage"] = "Az időpontot sikeresen lefoglaltuk!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Foglalási hiba");
                ModelState.AddModelError(string.Empty, ex.Message);
                return View("Index", await BuildPageViewModelAsync(model.Form, cancellationToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Váratlan hiba időpont mentésekor");
                ModelState.AddModelError(string.Empty, "Váratlan hiba történt. Próbálja meg később.");
                return View("Index", await BuildPageViewModelAsync(model.Form, cancellationToken));
            }
        }

        private async Task<AppointmentPageViewModel> BuildPageViewModelAsync(AppointmentFormViewModel? form, CancellationToken cancellationToken)
        {
            var userId = GetCurrentUserId();
            var appointments = await _appointmentManager.GetForUserAsync(userId, cancellationToken);

            return new AppointmentPageViewModel
            {
                UpcomingAppointments = appointments
                    .Select(a => new AppointmentListItemViewModel
                    {
                        AppointmentDateTime = a.AppointmentDateTime,
                        Note = a.Note,
                        Created = a.Created
                    })
                    .ToList(),
                Form = form ?? new AppointmentFormViewModel()
            };
        }

        private int GetCurrentUserId()
        {
            var value = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException("A felhasználó azonosítója nem érhető el.");
            }

            return int.Parse(value);
        }
    }
}

