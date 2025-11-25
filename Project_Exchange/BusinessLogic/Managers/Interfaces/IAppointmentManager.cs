using BusinessLogic.Entities;

namespace BusinessLogic.Managers.Interfaces
{
    public interface IAppointmentManager
    {
        Task<IReadOnlyCollection<Appointment>> GetForUserAsync(int userId, CancellationToken cancellationToken = default);
        Task<Appointment> CreateAsync(int userId, DateTime appointmentDateTime, string note, CancellationToken cancellationToken = default);
        Task<bool> IsSlotAvailableAsync(DateTime appointmentDateTime, CancellationToken cancellationToken = default);
    }
}

