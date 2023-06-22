using System.Data;
using System.Diagnostics;
using AutoMapper;
using Azure;
using Azure.Core;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Common;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Response.VacancyDTOs;
using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Infrastructure.Utils.Extensions;
using EmploymentSystem.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace EmploymentSystem.Infrastructure.Services;

public class ApplicantServices : BaseService<ApplicantServices>, IApplicantServices
{


    public ApplicantServices(IMapper mapper,
        ILogger<ApplicantServices> logger,
        IApplicationDbContext context) : base(mapper, logger, context)
    {
    }

    public async Task<VacanciesResponseDto> Search(VacancySearchRequestDto searchRequest)
    {
        var response = new VacanciesResponseDto()
        {
            ResponseMessage = Resource.NotFound,
            ResponseCode = ResponseCodes.NotFound
        };
        try
        {
            var vacanciesList = await Context.Vacancies
                .Where(x => x.IsActive)
                .WhereIf(!string.IsNullOrEmpty(searchRequest.Title) && !string.IsNullOrWhiteSpace(searchRequest.Title),
                    x => x.Title.Equals(searchRequest.Title))
                .WhereIf(!string.IsNullOrEmpty(searchRequest.VacancyNumber) && !string.IsNullOrWhiteSpace(searchRequest.Title),
                    x => x.VacancyNumber.Equals(searchRequest.VacancyNumber))
                .ToListAsync();

            if (vacanciesList.Any())
            {
                response.ResponseMessage = Resource.Sucess;
                response.ResponseCode = ResponseCodes.ProcessedSuccessfully;

                response.Vacancies = Mapper.Map<IEnumerable<VacancyResponseDto>>(vacanciesList);
            }
        }
        catch (Exception ex)
        {
            return (VacanciesResponseDto)LogException(ex);
        }
        return response;
    }

    public async Task<ResponseDto> Apply(int applicantId, int vacancyId)
    {
        var response = new ResponseDto()
        {
            ResponseMessage = Resource.Failed,
            ResponseCode = ResponseCodes.FailedToProcess
        };
        await using var transaction = await Context.Transaction;
        try
        {
            
            if (!await HasAppliedToday(applicantId))
            {
                var application = new Domain.Entities.Application(vacancyId);
                var added = Context.Applications.Add(application);
                if (added.State != EntityState.Added)
                {
                    await transaction.RollbackAsync();
                    return response;
                }

                await Context.SaveChangesAsync(default);

                return await AssignApplicantToApplication(applicantId, added.Entity.Id, transaction);
            }

            response.ResponseMessage = Resource.YouShouldApplayAfter24Hours;
            response.ResponseCode = ResponseCodes.DuplicatedApplication;
        }

        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return LogException(ex);
        }

        return response;
    }


    #region Utils

    private async Task<ResponseDto> AssignApplicantToApplication(int applicantId, int applicationId, IDbContextTransaction transaction)
    {
        var response = new ResponseDto()
        {
            ResponseMessage = Resource.Failed,
            ResponseCode = ResponseCodes.FailedToProcess
        };
        try
        {
            var mapApplicationAndApplicant = new ApplicantApplications(applicantId, applicationId);

            var mapped = await Context.ApplicantApplications.AddAsync(mapApplicationAndApplicant);
            if (mapped.State == EntityState.Added)
            {
                await Context.SaveChangesAsync(default);
                response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
                response.ResponseMessage = Resource.Sucess;
                await transaction.CommitAsync();
                return response;
            }

            await transaction.RollbackAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return LogException(ex);
        }

        return response;
    }
    private async Task<bool> HasAppliedToday(int applicantId)
    {

        //var hasAppliedTodayQueryable = Context.ApplicantsPerVacancy
        //    .Where(ap => ap.ApplicantId == applicantId && ap.AppliedAt.Day.Equals(DateTime.Now.Day));

        //var hasAppliedToday = await hasAppliedTodayQueryable.ToListAsync();

        var hasAppliedTodayQueryable = Context.ApplicantApplications
            .Where(ap => ap.ApplicantId == applicantId)
            .OrderByDescending(x => x.LastAppliedDateTime);

        var hasAppliedToday = await hasAppliedTodayQueryable.Where(x => x.LastAppliedDateTime > DateTime.Now.AddDays(-1)).ToListAsync();
        if (hasAppliedToday.Any())
        {
            return true;
        }

        return false;
    }


    #endregion

}