using Infrastructure.Auth;
using Infrastructure.Common;
using Infrastructure.MapsterConfig;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Init;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            MapsterSettings.Configure();
            return services
                .AddMediatR(Assembly.GetExecutingAssembly())
                .AddAuth(config)
                .AddPersistence(config)
                .AddRouting(options => options.LowercaseUrls = true)
                .AddServices();
        }

        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            using var scope = services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
                .InitializeDatabasesAsync(cancellationToken);
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config)
        {
            return builder
           .UseStaticFiles()
           .UseRouting()
           .UseAuthentication()
           .UseAuthorization();
        }
    }
}
