using Application.Common.Interface;
using Domain.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Init;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infrastructure.Seeders
{
    public class AuthorObject
    {
        public string Name { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
    }

    public class AuthorSeeder : ICustomSeeder
    {
        private readonly ISerializerService _serializerService;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AuthorSeeder> _logger;

        public AuthorSeeder(ISerializerService serializerService, ILogger<AuthorSeeder> logger, ApplicationDbContext db)
        {
            _serializerService = serializerService;
            _logger = logger;
            _db = db;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken)
        {
            string? authorPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string authorsData = await File.ReadAllTextAsync(authorPath + "/Seeders/authors.json", cancellationToken);
            List<AuthorObject> authors = _serializerService.Deserialize<List<AuthorObject>>(authorsData);

            if (authors != null)
            {
                Microsoft.EntityFrameworkCore.DbSet<Author> existingType = _db.Authors;
                IEnumerable<AuthorObject> missing = authors.Where(lt => !existingType.Any(l => l.Name == lt.Name));

                if (missing.Any())
                {
                    _logger.LogInformation("Started to Seed Authors.");
                    foreach (AuthorObject author in missing)
                    {
                        Author newAuthor = new Author(author.Name, author.DateOfBirth, null);
                        await _db.Authors.AddAsync(newAuthor, cancellationToken);
                    }

                    await _db.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("Seeded Authors.");
                }
            }
        }
    }
}

