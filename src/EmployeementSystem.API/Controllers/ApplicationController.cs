using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AutoMapper.Configuration.Annotations;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Response.VacancyDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmploymentSystem.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Applicant")]
public class ApplicationController : APIBaseController<ApplicationController>
{
    private readonly IApplicantServices _services;
    public ApplicationController(IApplicantServices services)
    {
        _services = services;
    }



    [HttpPost("SearchForVacancy")]
    public async Task<ActionResult<VacanciesResponseDto>> Search(VacancySearchRequestDto? request)
    {
        return await _services.Search(request);
    }

    [HttpPost("Apply/{vacancyId}")]
    public async Task<ActionResult<ResponseDto>> Apply(int vacancyId)
    {
        var actor = GetActor();
        int.TryParse(actor, out int applicantId);
        return await _services.Apply(applicantId, vacancyId);
    }
}