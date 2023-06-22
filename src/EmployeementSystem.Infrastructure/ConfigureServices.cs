using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Infrastructure.Persistence;
using EmploymentSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmploymentSystem.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IEmployerServices, EmployerService>();
        services.AddScoped<IApplicantServices, ApplicantServices>();

        //Add DbContext using SQL server 
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<Application.Contracts.IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}
