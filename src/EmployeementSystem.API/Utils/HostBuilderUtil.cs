using EmploymentSystem.Domain.lookups;
using EmploymentSystem.Infrastructure;
using EmploymentSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmploymentSystem.API.Utils
{
    public class HostBuilderUtil
    {
        public static IHostBuilder CreateHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddInfrastructureServices(configuration);

                    // Other service configurations...

                    SeedRoles(services.BuildServiceProvider().CreateScope().ServiceProvider);
                });

        private static void SeedRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            foreach (var role in Enum.GetValues(typeof(UserType)))
            {
                if (!roleManager.RoleExistsAsync(role.ToString()).Result)
                {
                    var newRole = new IdentityRole(role.ToString());
                    roleManager.CreateAsync(newRole).Wait();
                }
            }
        }
    }
}
