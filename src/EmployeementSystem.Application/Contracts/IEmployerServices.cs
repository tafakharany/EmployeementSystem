using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Response.ApplicantDTOs;

namespace EmploymentSystem.Application.Contracts
{
    public interface IEmployerServices
    {
        Task<ResponseDto> CreateVacancy(CreateVacancyRequestDto vacancy);
        Task<ResponseDto> UpdateVacancy(int vacancyId, UpdateVacancyRequestDto vacancy);
        Task<ResponseDto> DeactivateVacancy(DeactivateVacancyDto request);
        Task<ApplicantListResponseDTO> ViewVacancyApplicantList(int vacancyId);
    }
}
