using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using EmploymentSystem.Application.Utils;
using EmploymentSystem.Resources;
using FluentValidation;

namespace EmploymentSystem.Application.DTOs.Requests.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().WithMessage(x => $"{{PropertyName}}: {Resource.NotValidEmail}")
          .Matches(RegularExpressions.Email).WithMessage(x => $"{{PropertyName}}: {Resource.NotValidEmail}");

        RuleFor(x => x.MobileNumber)
            .MinimumLength(11).WithMessage(x => $"{{PropertyName}}: {Resource.MinLengthIs11}")
            .MaximumLength(15).WithMessage(x => $"{{PropertyName}}: {Resource.MaxLengthIs15}")
            .Matches(RegularExpressions.MobileNumber).WithMessage(x => $"{{PropertyName}}: {Resource.InvalidMobileNumber}");

        RuleFor(x => x.Password).MinimumLength(8).WithMessage(x => $"{{PropertyName}}: {Resource.PwdMinLentgh}")
            .Matches(RegularExpressions.Password).WithMessage(x => $"{{PropertyName}}: {Resource.InvalidPWD}");

        RuleFor(x => x.FirstName).NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}");
        RuleFor(x => x.LastName).NotNull().WithMessage(x => $"{{PropertyName}}: {Resource.Required}");


    }
}

