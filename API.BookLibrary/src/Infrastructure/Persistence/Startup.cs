﻿using Application.Repository;
using Domain.Common.Contracts;
using Infrastructure.Common;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Init;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Persistence
{
    internal static class Startup
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(Startup));

        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
        {
            var databaseSettings = config.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            string? rootConnectionString = databaseSettings.ConnectionString;
            if (string.IsNullOrEmpty(rootConnectionString))
            {
                throw new InvalidOperationException("DB ConnectionString is not configured.");
            }

            string? dbProvider = databaseSettings.DBProvider;
            if (string.IsNullOrEmpty(dbProvider))
            {
                throw new InvalidOperationException("DB Provider is not configured.");
            }

            _logger.Information($"Current DB Provider : {dbProvider}");

            return services
                .Configure<DatabaseSettings>(config.GetSection(nameof(DatabaseSettings)))

                .AddDbContext<ApplicationDbContext>(m => m.UseDatabase(dbProvider, rootConnectionString))

                .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
                .AddTransient<ApplicationDbInitializer>()
                .AddTransient<ApplicationDbSeeder>()
                .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
                .AddTransient<CustomSeederRunner>()
                .AddRepositories();

        }

        internal static DbContextOptionsBuilder UseDatabase(this DbContextOptionsBuilder builder, string dbProvider, string connectionString)
        {
            switch (dbProvider.ToLowerInvariant())
            {
                case DbProviderKeys.SqlServer:
                    return builder.UseSqlServer(connectionString, e =>
                         e.MigrationsAssembly("Migrators.MSSQL"));

                default:
                    throw new InvalidOperationException($"DB Provider {dbProvider} is not supported.");
            }
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Add Repositories
            services.AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));

            foreach (var aggregateRootType in
                typeof(IAggregateRoot).Assembly.GetExportedTypes()
                    .Where(t => typeof(IAggregateRoot).IsAssignableFrom(t) && t.IsClass)
                    .ToList())
            {
                // Add ReadRepositories.
                services.AddScoped(typeof(IReadRepository<>).MakeGenericType(aggregateRootType), sp =>
                    sp.GetRequiredService(typeof(IRepository<>).MakeGenericType(aggregateRootType)));

            }

            return services;
        }
    }
}
