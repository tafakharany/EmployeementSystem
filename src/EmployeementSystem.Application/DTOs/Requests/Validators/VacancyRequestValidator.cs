using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.Utils;
using EmploymentSystem.Resources;

namespace EmploymentSystem.Application.DTOs.Requests.Validators;

public class VacancyRequestValidator : AbstractValidator<CreateVacancyRequestDto>
{
    public VacancyRequestValidator()
    {
        RuleFor(x => x.Description).NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}");


        RuleFor(x => x.Title)
            .NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}");

    }
}