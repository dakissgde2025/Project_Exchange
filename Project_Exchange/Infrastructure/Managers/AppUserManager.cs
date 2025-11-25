using BusinessLogic.Entities;
using BusinessLogic.Managers.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Managers
{
    public class AppUserManager : IAppUserManager
    {
        private readonly ProjectExchangeDbContext _dbContext;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        private readonly ILogger<AppUserManager> _logger;

        public AppUserManager(
            ProjectExchangeDbContext dbContext,
            IPasswordHasher<AppUser> passwordHasher,
            ILogger<AppUserManager> logger)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<AppUser> CreateAsync(string email, string password, string? phoneNumber, CancellationToken cancellationToken = default)
        {
            if (await EmailExistsAsync(email, cancellationToken))
            {
                throw new InvalidOperationException("A felhasználói fiók már létezik a megadott email címmel.");
            }

            var entity = new AppUser
            {
                Email = email.Trim(),
                PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber.Trim(),
                Created = DateTime.UtcNow
            };

            entity.Password = _passwordHasher.HashPassword(entity, password);

            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Új felhasználó létrehozva: {Email}", email);

            return entity;
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AppUser>()
                .AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AppUser>()
                .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<AppUser?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<AppUser>()
                .FirstOrDefaultAsync(u => u.ID == userId, cancellationToken);
        }

        public async Task<AppUser?> ValidateCredentialsAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await GetByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                return null;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success ? user : null;
        }
    }
}

