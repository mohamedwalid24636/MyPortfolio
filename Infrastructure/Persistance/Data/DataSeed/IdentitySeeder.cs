using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Options;

namespace Persistance.Data.DataSeed
{
    /// <summary>
    /// Creates the admin account described by the <c>AdminSeed</c> configuration section.
    ///
    /// Seeding is create-only: an account that already exists is left completely alone, so this can
    /// run on every startup without ever reverting a password change made through the admin panel.
    /// </summary>
    public static class IdentitySeeder
    {
        public static async Task SeedAdminUserAsync(this IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<AdminSeedOptions>>().Value;
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(IdentitySeeder).FullName!);

            if (!options.IsConfigured)
            {
                logger.LogWarning(
                    "No admin account seeded: set AdminSeed:Email and AdminSeed:Password to create one.");

                return;
            }

            // A scope is required because UserManager is scoped and this runs against the root
            // provider, which lives for the whole application rather than a single request.
            using var scope = services.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (await userManager.FindByEmailAsync(options.Email) is not null)
            {
                logger.LogInformation("Admin account {Email} already exists; seeding skipped.", options.Email);

                return;
            }

            var admin = new ApplicationUser
            {
                UserName = options.Email,
                Email = options.Email,
                // There is no confirmation email to click, so the account starts out confirmed.
                EmailConfirmed = true,
            };

            // CreateAsync hashes the password with Identity's PasswordHasher before it is stored.
            var result = await userManager.CreateAsync(admin, options.Password);

            if (!result.Succeeded)
            {
                var failures = string.Join("; ", result.Errors.Select(error => error.Description));

                throw new InvalidOperationException(
                    $"Could not seed the admin account '{options.Email}': {failures}");
            }

            logger.LogInformation("Seeded admin account {Email}.", options.Email);
        }
    }
}
