using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploymentSystem.Infrastructure.Extensions;
using Serilog;

namespace EmploymentSystem.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IEmployerServices, EmployerService>();
            services.AddScoped<IApplicantServices, ApplicantServices>();
            if (string.IsNullOrEmpty(connectionString))
            {
                Log.Logger.Error("Services Error: connection string is null, hangFire and dbContext not set");
                return services;
            }
            services.AddDbContextServices(connectionString);
            services.AddHangFireService(connectionString);

            return services;
        }
    }
}
