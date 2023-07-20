using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace EmploymentSystem.API.ServiceExtensions;

public static class IdentityServicesExtension
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        services.AddScoped<UserManager<User>>();
        services.AddAuthorization();
        return services;
    }
}
