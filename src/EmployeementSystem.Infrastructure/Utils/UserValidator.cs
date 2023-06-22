using Microsoft.AspNetCore.Identity;

namespace EmploymentSystem.Infrastructure.Utils;

public class CustomUserValidator<T> : UserValidator<T> where T : class
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<T> manager, T user)
    {
        var result = await base.ValidateAsync(manager, user);
        return result;
    }
}
