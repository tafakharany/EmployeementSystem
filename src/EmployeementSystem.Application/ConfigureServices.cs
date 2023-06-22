using EmploymentSystem.Application.DTOs.Requests.Validators;
using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.Mappers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace EmploymentSystem.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Profiles));
        services.AddTransient<IValidator<RegisterRequestDto>, RegisterRequestValidator>();
        services.AddTransient<IValidator<LoginRequestDto>, LoginRequestValidator>();
        services.AddTransient<IValidator<CreateVacancyRequestDto>, VacancyRequestValidator>();
        services.AddTransient<IValidator<DeactivateVacancyDto>, DeactivateRequestValidator>();
        return services;
    }
}
