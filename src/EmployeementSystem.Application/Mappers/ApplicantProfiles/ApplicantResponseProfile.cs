using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploymentSystem.Application.DTOs.Response.ApplicantDTOs;
using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Domain.Views;

namespace EmploymentSystem.Application.Mappers.ApplicantProfiles
{
    public class ApplicantResponseProfile : Profiles
    {
        public ApplicantResponseProfile()
        {
            CreateMap<User, ApplicantDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => string.Concat(src.FirstName, " ", src.LastName)))
                .ForMember(dest => dest.AppliedAt,
                    opt => opt.MapFrom(src => src.Applications.Select(app => app.LastAppliedDateTime)));


            CreateMap<ViewApplicantsList, ApplicantDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicantId))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.ApplicantName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.ApplicantEmail));
        }
    }
}
