using EmploymentSystem.API.ServiceExtensions;
using EmploymentSystem.Application;
using EmploymentSystem.Infrastructure;
using Hangfire;
using Serilog;


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
WebApplication app;

try
{   // Add services to the container.-
    builder.Services.AddControllers();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    var services = builder.Services;
    services.AddApplicationServices();
    services.AddJWtService(builder.Configuration);
    services.AddIdentityService(builder.Configuration);
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerWithAuthorization();
    services.AddRateLimiterService(configuration);
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    builder.Host.UseSerilog(logger);

    app = builder.Build();

    logger.Information($"EmploymentSystem API Starting Up! in {environment} Environment");
}
catch (Exception ex)
{
    logger.Fatal(ex, "The EmploymentSystem API Failed to Start Correctly.");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (app.Environment.IsProduction())
    app.UseHttpsRedirection();

app.UseRateLimiter();
app.UseRequestLocalization();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
//app.UseHangfireDashboard(options =>
//{
//    options.Username("Taha");
//    options.Password("123P@$$w0rd");
//});
app.UseHangfireDashboard();
app.MapHangfireDashboard();

app.Run();