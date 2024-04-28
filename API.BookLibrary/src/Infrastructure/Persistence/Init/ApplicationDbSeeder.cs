using Domain.Models.Identity;
using Infrastructure.Common.Roles;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Auth;
using Shared.PermissionSettings;
using Shared.UserConstants;

namespace Infrastructure.Persistence.Init
{
    internal class ApplicationDbSeeder
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomSeederRunner _seederRunner;
        private readonly ILogger<ApplicationDbSeeder> _logger;

        public ApplicationDbSeeder(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _seederRunner = seederRunner;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken)
        {
            await SeedRolesAsync(dbContext);
            await SeedAdminUserAsync();
            await SeedBasicUserAsync();
            await _seederRunner.RunSeedersAsync(cancellationToken);
        }

        private async Task SeedRolesAsync(ApplicationDbContext dbContext)
        {
            foreach (string roleName in UserRoles.DefaultUserRoles)
            {
                if (await _roleManager.Roles.SingleOrDefaultAsync(r => r.Name == roleName)
                    is not ApplicationRole role)
                {
                    // Create the role
                    _logger.LogInformation("Seeding {role} Role for default user", roleName);

                    role = new ApplicationRole(roleName, $"{roleName} Role for Default User");
                    await _roleManager.CreateAsync(role);
                }

                // Assign permissions
                if (roleName == UserRoles.Basic)
                {
                    await AssignPermissionsToRoleAsync(dbContext, Permissions.Basic, role);
                }
                else if (roleName == UserRoles.Admin)
                {
                    await AssignPermissionsToRoleAsync(dbContext, Permissions.Admin, role);
                }
            }
        }

        private async Task AssignPermissionsToRoleAsync(ApplicationDbContext dbContext, IReadOnlyList<Permission> permissions, ApplicationRole role)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var permission in permissions)
            {
                if (!currentClaims.Any(c => c.Type == ExtendedClaims.Permission && c.Value == permission.Name))
                {
                    _logger.LogInformation("Seeding {role} Permission '{permission}'", role.Name, permission.Name);
                    dbContext.RoleClaims.Add(new ApplicationRoleClaim
                    {
                        RoleId = role.Id,
                        ClaimType = ExtendedClaims.Permission,
                        ClaimValue = permission.Name,
                        CreatedBy = "ApplicationDbSeeder"
                    });
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private async Task SeedBasicUserAsync()
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == UserDetailsConstants.Email)
                is not ApplicationUser basicUser)
            {
                basicUser = new ApplicationUser
                {
                    FirstName = UserDetailsConstants.FirstName.ToLowerInvariant(),
                    LastName = UserDetailsConstants.LastName,
                    Email = UserDetailsConstants.Email,
                    UserName = UserDetailsConstants.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };

                _logger.LogInformation("Seeding Default Basic User");
                var result = await _userManager.CreateAsync(basicUser, UserDetailsConstants.DefaultPassword);

                // Assign role to user
                if (result.Succeeded && !await _userManager.IsInRoleAsync(basicUser, UserRoles.Basic))
                {
                    _logger.LogInformation("Assigning user Role to Basic User");
                    await _userManager.AddToRoleAsync(basicUser, UserRoles.Basic);
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == AdminUserDetailsConstants.Email)
                is not ApplicationUser adminUser)
            {
                adminUser = new ApplicationUser
                {
                    FirstName = AdminUserDetailsConstants.FirstName.ToLowerInvariant(),
                    LastName = AdminUserDetailsConstants.LastName,
                    Email = AdminUserDetailsConstants.Email,
                    UserName = AdminUserDetailsConstants.Email,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    IsActive = true
                };

                _logger.LogInformation("Seeding Default Admin User");
                var result = await _userManager.CreateAsync(adminUser, AdminUserDetailsConstants.DefaultPassword);

                // Assign role to user
                if (result.Succeeded && !await _userManager.IsInRoleAsync(adminUser, UserRoles.Admin))
                {
                    _logger.LogInformation("Assigning Admin Role to Admin User");
                    await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
                }
            }

        }
    }
}
