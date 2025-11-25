using BusinessLogic.Entities;
using BusinessLogic.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Managers
{
    public class AppointmentManager : IAppointmentManager
    {
        private readonly ProjectExchangeDbContext _dbContext;
        private readonly ILogger<AppointmentManager> _logger;

        public AppointmentManager(ProjectExchangeDbContext dbContext, ILogger<AppointmentManager> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Appointment> CreateAsync(int userId, DateTime appointmentDateTime, string note, CancellationToken cancellationToken = default)
        {
            if (!await IsSlotAvailableAsync(appointmentDateTime, cancellationToken))
            {
                throw new InvalidOperationException("A kiválasztott időpont már foglalt.");
            }

            var entity = new Appointment
            {
                AppUser_ID = userId,
                AppointmentDateTime = appointmentDateTime,
                Note = note.Trim(),
                Created = DateTime.UtcNow
            };

            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Új időpont lefoglalva: {DateTime} felhasználó: {UserId}", appointmentDateTime, userId);

            return entity;
        }

        public async Task<IReadOnlyCollection<Appointment>> GetForUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Appointment>()
                .Where(a => a.AppUser_ID == userId)
                .OrderBy(a => a.AppointmentDateTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsSlotAvailableAsync(DateTime appointmentDateTime, CancellationToken cancellationToken = default)
        {
            return !await _dbContext.Set<Appointment>()
                .AnyAsync(a => a.AppointmentDateTime == appointmentDateTime, cancellationToken);
        }
    }
}

