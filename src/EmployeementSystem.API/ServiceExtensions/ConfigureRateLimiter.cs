using System.Threading.RateLimiting;
using EmploymentSystem.Resources;

namespace EmploymentSystem.API.ServiceExtensions;

public static class ConfigureRateLimiter
{
    public static IServiceCollection AddRateLimiterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(partitionKey: httpContext.Request.Headers.Host.ToString(), partition =>
                    new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 1,
                        AutoReplenishment = true,
                        Window = TimeSpan.FromSeconds(10)
                    });
            });
            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = 429;
                await context.HttpContext.Response.WriteAsync(Resource.TooManyRequests, cancellationToken: token);
            };
        }
        );
        return services;
    }
}