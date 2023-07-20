using EmploymentSystem.Application.DTOs.Requests.IdentityDTOs;
using EmploymentSystem.Domain.Entities;

namespace EmploymentSystem.Application.Mappers.IdentityProfiles;

public class UserProfile : Profiles
{
    public UserProfile()
    {
        #region Register
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.Email));
            //.ForPath(dest=>dest.Role.Id, opt=>opt.MapFrom(src=>src.UserType));
        #endregion
    }
}
