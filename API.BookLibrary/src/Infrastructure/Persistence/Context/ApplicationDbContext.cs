using Domain.Models;
using Domain.Models.Identity;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : BaseDbContext
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<DatabaseSettings> dbSettings) : base(options, dbSettings)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
        public DbSet<ApplicationRole> ApplicationRoles => Set<ApplicationRole>();
        public DbSet<ApplicationRoleClaim> ApplicationClaims => Set<ApplicationRoleClaim>();

        public DbSet<Book> Books => Set<Book>();

        public DbSet<Author> Authors => Set<Author>();

        public DbSet<Publisher> Publishers => Set<Publisher>();

        public DbSet<Lookup> Lookups => Set<Lookup>();

        public DbSet<LookupType> LookupTypes => Set<LookupType>();

        public DbSet<Rental> Rentals => Set<Rental>();

        public DbSet<Reservation> Reservations => Set<Reservation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
        }
    }
}
