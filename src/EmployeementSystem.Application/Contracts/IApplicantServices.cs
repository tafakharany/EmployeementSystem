using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Response.VacancyDTOs;

namespace EmploymentSystem.Application.Contracts
{
    public interface IApplicantServices
    {
        Task<VacanciesResponseDto> Search(VacancySearchRequestDto searchRequest);
        Task<ResponseDto> Apply(int applicantId, int vacancyId);
    }
}
