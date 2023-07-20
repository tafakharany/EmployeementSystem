using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentSystem.API.Filters;

public class ValidateAntiForgeryTokenAuthorizationFilter : IAuthorizationFilter
{
    private readonly string _antiForgeryHeaderName;
    private readonly string _antiForgeryCookieName;

    public ValidateAntiForgeryTokenAuthorizationFilter()
    {
        _antiForgeryHeaderName = "X-CSRF-TOKEN"; // The name of the header that includes the antiForgery token.
        _antiForgeryCookieName = ".AspNetCore.AntiForgery"; // The name of the cookie that includes the antiForgery token.
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var antiforgery = context.HttpContext.RequestServices.GetRequiredService<IAntiforgery>();

        var headers = context.HttpContext.Request.Headers;
        var cookies = context.HttpContext.Request.Cookies;

        if (headers.ContainsKey(_antiForgeryHeaderName) && cookies.ContainsKey(_antiForgeryCookieName))
        {
            var headerToken = headers[_antiForgeryHeaderName];
            var cookieToken = cookies[_antiForgeryCookieName];

            antiforgery.ValidateRequestAsync(context.HttpContext).Wait();
        }
        else
        {
            context.Result = new StatusCodeResult(403);
        }
    }
}
