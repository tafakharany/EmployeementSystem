using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Common;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using EmploymentSystem.Domain.lookups;
using EmploymentSystem.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EmploymentSystem.API.Controllers;

public class IdentityController : APIBaseController<IdentityController>
{
    private readonly IValidator<RegisterRequestDto> _registrationValidator;
    private readonly IValidator<LoginRequestDto> _loginValidator;
    private readonly IIdentityService _service;
    public IdentityController(IValidator<LoginRequestDto> loginValidator,
        IValidator<RegisterRequestDto> registrationValidator,
        IIdentityService service)
    {
        _loginValidator = loginValidator;
        _registrationValidator = registrationValidator;
        _service = service;
    }

    [HttpPost("registerNewUser")]
    public async Task<ActionResult<RegisterResponseDto>> SignUp([FromBody] RegisterRequestDto request)
    {
        RegisterResponseDto response = new()
        {
            ResponseMessage = Resource.Failed,
            ResponseCode = ResponseCodes.FailedToProcess
        };

        try
        {
            var validationResult = _registrationValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.ResponseCode = ResponseCodes.ValidationError;
                response.ResponseMessage = string.Join(",", SetErrorMessage(validationResult.Errors));
                return BadRequest(response);
            }

            response = await _service.RegisterUser(request);

        }
        catch (Exception ex)
        {
            
        }

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<RegisterResponseDto>> SignIn([FromBody] LoginRequestDto request)
    {
        LoginResponseDto response = new()
        {
            ResponseMessage = Resource.Failed,
            ResponseCode = ResponseCodes.FailedToProcess
        };

        try
        {
            var validationResult = _loginValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.ResponseCode = ResponseCodes.ValidationError;
                response.ResponseMessage = string.Join(",", SetErrorMessage(validationResult.Errors));
                return BadRequest(response);
            }

            response = await _service.Login(request);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, ex.Message, ex.StackTrace);
            response.ResponseCode = ResponseCodes.GeneralError;
            response.ResponseMessage = Resource.GeneralError;
        }

        return Ok(response);
    }
}
