using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Requests.Validators;

public class DeactivateRequestValidator : AbstractValidator<DeactivateVacancyDto>
{
    public DeactivateRequestValidator()
    {
        RuleFor(x => x.VacancyId).NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}")
            .NotEqual(0).WithMessage(x => $"{{PropertyName}}: {Resource.Required}");

    }
}