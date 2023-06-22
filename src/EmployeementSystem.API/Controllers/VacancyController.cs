using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Azure;
using Azure.Core;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Common;
using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Response.ApplicantDTOs;
using EmploymentSystem.Domain.lookups;
using EmploymentSystem.Resources;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentSystem.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Employer")]
public class VacancyController : APIBaseController<VacancyController>
{
    private readonly IValidator<CreateVacancyRequestDto> _requestValidator;
    private readonly IValidator<DeactivateVacancyDto> _deactivateValidator;
    private readonly IEmployerServices _service;
    public VacancyController(IValidator<LoginRequestDto> loginValidator,
        IValidator<CreateVacancyRequestDto> requestValidator,
        IValidator<DeactivateVacancyDto> deactiveValidator,
        IEmployerServices service)
    {
        _requestValidator = requestValidator;
        _service = service;
        _deactivateValidator = deactiveValidator;
    }

    [HttpPost("PostVacancy")]
    public async Task<ActionResult<ResponseDto>> CreateNewVacancy(CreateVacancyRequestDto request)
    {
        var response = new ResponseDto()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };
        var actor = GetActor();
        if (!string.IsNullOrEmpty(actor))
        {
            request.CreatedBy = Convert.ToInt32(actor);
        }
        var validationResult = await _requestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            response.ResponseCode = ResponseCodes.ValidationError;
            response.ResponseMessage = string.Join(",", SetErrorMessage(validationResult.Errors));
            return BadRequest(response);
        }

        response = await _service.CreateVacancy(request);

        return Ok(response);
    }

    [HttpPut("UpdateVacancy/{vacancyId}")]
    public async Task<ActionResult<ResponseDto>> UpdateVacancy(int vacancyId, UpdateVacancyRequestDto request)
    {
        var response = new ResponseDto()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };
        var actor = GetActor();
        if (!string.IsNullOrEmpty(actor))
        {
            request.UpdatedBy = Convert.ToInt32(actor);
        }

        if (vacancyId <= 0)
        {
            response.ResponseCode = ResponseCodes.ValidationError;
            response.ResponseMessage = " Vacancy Id is Required field";
            return BadRequest(response);
        }

        response = await _service.UpdateVacancy(vacancyId, request);

        return Ok(response);
    }

    [HttpPut("DeactivateVacancy/{vacancyId}")]
    public async Task<ActionResult<ResponseDto>> DeactivateVacancy(int vacancyId)
    {
        var response = new ResponseDto()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };
        DeactivateVacancyDto request = new DeactivateVacancyDto();
        request.VacancyId = vacancyId;
        var actor = GetActor();
        if (!string.IsNullOrEmpty(actor))
        {
            request.EmployerId = Convert.ToInt32(actor);
        }
        var validationResult = await _deactivateValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            response.ResponseCode = ResponseCodes.ValidationError;
            response.ResponseMessage = string.Join(",", SetErrorMessage(validationResult.Errors));
            return BadRequest(response);
        }

        response = await _service.DeactivateVacancy(request);

        return Ok(response);
    }


    [HttpGet("GetApplicantsForVacancy/{vacancyId}")]
    public async Task<ActionResult<ApplicantListResponseDTO>> GetAllApplicantsForVacancy(int vacancyId)
    {
        var response = new ApplicantListResponseDTO()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };

        if (vacancyId == 0)
        {
            response.ResponseCode = ResponseCodes.ValidationError;
            response.ResponseMessage = " Vacancy Id is Required field";
            return BadRequest(response);
        }

        response = await _service.ViewVacancyApplicantList(vacancyId);

        return Ok(response);
    }
}