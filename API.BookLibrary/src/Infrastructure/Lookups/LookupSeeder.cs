using Application.Common.Interface;
using Domain.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Init;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Lookups
{
    public class LookupObject
    {
        public string LookupType { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }

    public class LookupTypeObject
    {
        public string Name { get; set; } = string.Empty;
    }

    public class LookupSeeder : ICustomSeeder
    {
        private readonly ISerializerService _serializerService;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<LookupSeeder> _logger;

        public LookupSeeder(ISerializerService serializerService, ILogger<LookupSeeder> logger, ApplicationDbContext db)
        {
            _serializerService = serializerService;
            _logger = logger;
            _db = db;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? lookupTypePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string lookupTypesData = await File.ReadAllTextAsync(lookupTypePath + "/Lookups/lookuptypes.json", cancellationToken);
            List<LookupTypeObject> lookupTypes = _serializerService.Deserialize<List<LookupTypeObject>>(lookupTypesData);

            if (lookupTypes != null)
            {
                Microsoft.EntityFrameworkCore.DbSet<LookupType> existingType = _db.LookupTypes;
                IEnumerable<LookupTypeObject> missing = lookupTypes.Where(lt => !existingType.Any(l => l.Name == lt.Name));

                if (missing.Any())
                {
                    _logger.LogInformation("Started to Seed LookupTypes.");
                    foreach (LookupTypeObject lookupType in missing)
                    {
                        LookupType newLookupType = new(lookupType.Name);
                        await _db.LookupTypes.AddAsync(newLookupType, cancellationToken);
                    }

                    await _db.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Seeded LookupTypes.");
                }
            }

            string? lookupPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Here you can use your own logic to populate the database.
            // As an example, I am using a JSON file to populate the database.
            string lookupsData = await File.ReadAllTextAsync(lookupPath + "/Lookups/lookups.json", cancellationToken);
            List<LookupObject> lookups = _serializerService.Deserialize<List<LookupObject>>(lookupsData);

            if (lookups != null)
            {
                Microsoft.EntityFrameworkCore.DbSet<Lookup> existingLookup = _db.Lookups;

                IEnumerable<LookupObject> missing = lookups.Where(l => !existingLookup.Any(dl => dl.Label == l.Label && dl.Type!.Name == l.LookupType));

                if (missing.Any())
                {
                    _logger.LogInformation("Started to Seed Lookups.");
                    foreach (LookupObject lookup in missing)
                    {
                        LookupType? lookupType = _db.LookupTypes.FirstOrDefault(x => x.Name == lookup.LookupType);
                        if (lookupType != null)
                        {
                            Lookup newLookup = new Lookup(lookup.Label, lookupType.Id);
                            newLookup.Update(lookupType.Id, lookup.Label);
                            await _db.Lookups.AddAsync(newLookup, cancellationToken);
                        }
                    }

                    await _db.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Seeded Lookups.");
                }
            }
        }
    }
}
