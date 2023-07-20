using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using EmploymentSystem.Application.Utils;
using EmploymentSystem.Resources;
using FluentValidation;

namespace EmploymentSystem.Application.DTOs.Requests.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .Matches(RegularExpressions.Email).WithMessage(x => $"{{PropertyName}}: {Resource.NotValidEmail}");
    }
}

