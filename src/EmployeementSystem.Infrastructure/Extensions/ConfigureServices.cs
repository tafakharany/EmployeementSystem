using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Infrastructure.Persistence;
using EmploymentSystem.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmploymentSystem.Infrastructure.Extensions;

public static class AddDbServices
{
    public static IServiceCollection AddDbContextServices(this IServiceCollection services, string connectionString)
    {

        //Add DbContext using SQL server 

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<ApplicationDbContext>());
        return services;
    }
}
