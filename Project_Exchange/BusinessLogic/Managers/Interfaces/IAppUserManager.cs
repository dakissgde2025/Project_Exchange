using BusinessLogic.Entities;

namespace BusinessLogic.Managers.Interfaces
{
    public interface IAppUserManager
    {
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
        Task<AppUser> CreateAsync(string email, string password, string? phoneNumber, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<AppUser?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default);
    }
}

