﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Init
{
    internal class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly DatabaseSettings _dbSettings;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(IOptions<DatabaseSettings> dbSettings, IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
        {
            _dbSettings = dbSettings.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task InitializeDatabasesAsync(CancellationToken cancellationToken)
        {
      
            _logger.LogInformation("Init db");
            using var scope = _serviceProvider.CreateScope();

            // Then run the initialization in the new scope
            await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
                .InitializeAsync(cancellationToken);
        }

    }
}
