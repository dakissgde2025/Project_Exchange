using Microsoft.EntityFrameworkCore;
using BusinessLogic.Entities;

namespace Infrastructure
{
    public class ProjectExchangeDbContext : DbContext
    {
        public ProjectExchangeDbContext(DbContextOptions<ProjectExchangeDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<Appointment> Appointments => Set<Appointment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure AppUser entity
            modelBuilder.Entity<AppUser>(entity =>
            {
                // Table name exactly as in SQL script
                entity.ToTable("Appuser");

                // Primary key configuration
                entity.HasKey(e => e.ID);

                // Column configurations
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                    .HasColumnName("Email")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .HasColumnName("Password")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("PhoneNumber")
                    .HasMaxLength(255);

                entity.Property(e => e.Created)
                    .HasColumnName("Created")
                    .IsRequired()
                    .HasDefaultValueSql("GetDate()");

                entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified");

                // Navigation property configuration
                entity.HasMany(e => e.Appointments)
                    .WithOne(a => a.AppUser)
                    .HasForeignKey(a => a.AppUser_ID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Appointment entity
            modelBuilder.Entity<Appointment>(entity =>
            {
                // Table name exactly as in SQL script
                entity.ToTable("Appointments");

                // Primary key configuration
                entity.HasKey(e => e.ID);

                // Column configurations
                entity.Property(e => e.ID)
                    .HasColumnName("ID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AppointmentDateTime)
                    .HasColumnName("Appointment")
                    .IsRequired();

                entity.Property(e => e.Note)
                    .HasColumnName("Note")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.AppUser_ID)
                    .HasColumnName("AppUser_ID")
                    .IsRequired();

                entity.Property(e => e.Created)
                    .HasColumnName("Created")
                    .IsRequired()
                    .HasDefaultValueSql("GetDate()");

                entity.Property(e => e.LastModified)
                    .HasColumnName("LastModified");

                // Foreign key relationship
                entity.HasOne(e => e.AppUser)
                    .WithMany(u => u.Appointments)
                    .HasForeignKey(e => e.AppUser_ID)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
