using EmploymentSystem.Application.DTOs.Response;
using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;

namespace EmploymentSystem.Application.Contracts;

public interface IIdentityService
{
    Task<RegisterResponseDto> RegisterUser(RegisterRequestDto request);
    Task<LoginResponseDto> Login(LoginRequestDto request);
}
