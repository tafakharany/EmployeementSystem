using AutoMapper;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Application.DTOs.Common;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Response.ApplicantDTOs;
using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Infrastructure.Persistence;
using EmploymentSystem.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmploymentSystem.Infrastructure.Services;

public class EmployerService : BaseService<EmployerService>, IEmployerServices
{
    //private readonly ApplicationDbContext _context;
    public EmployerService(IMapper mapper,
        ILogger<EmployerService> logger,
        ApplicationDbContext context) : base(mapper, logger, context)
    {
        //_context = context;
    }

    public async Task<ResponseDto> CreateVacancy(CreateVacancyRequestDto vacancyRequest)
    {
        var response = new ResponseDto()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };
        try
        {
            //var affectedRows = 0;
            var vacancy = Mapper.Map<Vacancy>(vacancyRequest);
            var result = await Context.Vacancies.AddAsync(vacancy);
            if (result.State != EntityState.Added)
            {
                Logger.LogError(result.State.ToString());
                return response;
            }
            await Context.SaveChangesAsync(default);
            response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
            response.ResponseMessage = Resource.Sucess;
        }
        catch (Exception ex)
        {
            return LogException(ex);
        }
        return response;
    }

    public async Task<ResponseDto> UpdateVacancy(int vacancyId, UpdateVacancyRequestDto vacancy)
    {
        var response = new ResponseDto()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };
        try
        {
            var affectedRows = 0;
            var vacancyEntity = await Context.Vacancies.Where(v => v.Id == vacancyId).FirstOrDefaultAsync();
            if (vacancyEntity != null)
            {
                Mapper.Map(vacancy, vacancyEntity);
                affectedRows = await Context.SaveChangesAsync(default);
            }

            if (affectedRows != 0)
            {
                response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
                response.ResponseMessage = Resource.Sucess;
            }
        }
        catch (Exception ex)
        {
            return LogException(ex);
        }


        return response;
    }

    public async Task<ResponseDto> DeactivateVacancy(DeactivateVacancyDto request)
    {
        var response = new ResponseDto()
        {
            ResponseCode = ResponseCodes.FailedToProcess,
            ResponseMessage = Resource.Failed
        };
        try
        {
            var vacancyEntity = await Context.Vacancies.Where(v => v.Id == request.VacancyId).FirstOrDefaultAsync();
            if (vacancyEntity != null)
            {
                vacancyEntity.IsActive = false;
                vacancyEntity.UpdatedBy = request.EmployerId;
                vacancyEntity.UpdatedAt = DateTime.Now;

                var affectedRows = await Context.SaveChangesAsync(default);
                if (affectedRows != 0)
                {
                    response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
                    response.ResponseMessage = Resource.Sucess;
                }

                return response;
            }

            response.ResponseMessage = Resource.NotFound;
            response.ResponseCode = ResponseCodes.NotFound;

        }
        catch (Exception ex)
        {
            return LogException(ex);
        }
        return response;
    }

    //TODO: Make View to return the needed data with no hustle about the query
    public async Task<ApplicantListResponseDTO> ViewVacancyApplicantList(int vacancyId)
    {
        var response = new ApplicantListResponseDTO()
        {
            ResponseCode = ResponseCodes.NotFound,
            ResponseMessage = Resource.NotFound
        };

        try
        {
            var applicantList = await Context.ApplicantsPerVacancy.Where(v => v.VacancyId == vacancyId).ToListAsync();

            if (!applicantList.Any())
            {
                response.ResponseMessage = Resource.NotFound;
                response.ResponseCode = ResponseCodes.NotFound;
                return response;
            }

            response.ResponseMessage = Resource.Sucess;
            response.ResponseCode = ResponseCodes.ProcessedSuccessfully;
            response.Applicants = Mapper.Map<IEnumerable<ApplicantDto>>(applicantList);
        }
        catch (Exception ex)
        {
            return (ApplicantListResponseDTO)LogException(ex);
        }
        return response;
    }
}

