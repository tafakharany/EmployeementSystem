using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Infrastructure.Persistence;
using EmploymentSystem.Infrastructure.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmploymentSystem.Infrastructure.Extensions;

public static class ConfigureHangFireServices
{
    public static IServiceCollection AddHangFireService(this IServiceCollection services, string connectionString)
    {

        services.AddHangfire(options =>
        {
            options.UseSqlServerStorage(connectionString);
        });

        services.AddHangfireServer();
        services.AddTransient<IVacancyService, VacancyService>();
        return services;
    }
}