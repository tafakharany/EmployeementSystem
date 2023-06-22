using AutoMapper;
using Azure;
using Azure.Core;
using EmploymentSystem.Application.DTOs.Common;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Resources;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmploymentSystem.API.Controllers;

[ApiController]
[Route("[controller]")]
public class APIBaseController<T> : ControllerBase
{
    private IMapper _mapper = null!;
    private ILogger<T> _logger = null!;
    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<T>>();

    protected IEnumerable<string> SetErrorMessage(List<ValidationFailure> errors)
    {
        foreach (var error in errors)
            yield return error.ErrorMessage;
    }

    protected void LogExceptions(Exception ex, ResponseDto response)
    {
        Logger.LogError(ex, ex.Message, ex.StackTrace);
        response.ResponseCode = ResponseCodes.GeneralError;
        response.ResponseMessage = Resource.GeneralError;
    }


    protected string GetActor()
    {
        var user = HttpContext.User as ClaimsPrincipal;
        var actor = user.FindFirstValue(ClaimTypes.Actor);
        return actor;
    }
}
