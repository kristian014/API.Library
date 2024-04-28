using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Context
{
    public abstract class BaseDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
    {
        private readonly DatabaseSettings _dbSettings;

        protected BaseDbContext(DbContextOptions options, IOptions<DatabaseSettings> dbSettings)
            : base(options)
        {
            _dbSettings = dbSettings.Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();

            if (!string.IsNullOrWhiteSpace(_dbSettings?.ConnectionString) && !string.IsNullOrWhiteSpace(_dbSettings?.DBProvider))
            {
                optionsBuilder.UseDatabase(_dbSettings.DBProvider!, _dbSettings.ConnectionString);
            }
            else
            {
                throw new InvalidOperationException($"DB Provider or connection string is empty.");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}
