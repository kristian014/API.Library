using Application.Common.Interface;
using Domain.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Init;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Seeders
{
    public class PublisherObject
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PublisherSeeder : ICustomSeeder
    {
        private readonly ISerializerService _serializerService;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PublisherSeeder> _logger;

        public PublisherSeeder(ISerializerService serializerService, ILogger<PublisherSeeder> logger, ApplicationDbContext db)
        {
            _serializerService = serializerService;
            _logger = logger;
            _db = db;
        }
        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? publisherPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string publishersData = await File.ReadAllTextAsync(publisherPath + "/Seeders/publishers.json", cancellationToken);
            List<PublisherObject> publishers = _serializerService.Deserialize<List<PublisherObject>>(publishersData);

            if (publishers != null)
            {
                Microsoft.EntityFrameworkCore.DbSet<Publisher> existingType = _db.Publishers;
                IEnumerable<PublisherObject> missing = publishers.Where(lt => !existingType.Any(l => l.Name == lt.Name));

                if (missing.Any())
                {
                    _logger.LogInformation("Started to Seed Publisher.");
                    foreach (PublisherObject author in missing)
                    {
                        Publisher newPublisher = new Publisher(author.Name, null);
                        await _db.Publishers.AddAsync(newPublisher, cancellationToken);
                    }

                    await _db.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Seeded Publishers.");
                }
            }
        }
    }
}
