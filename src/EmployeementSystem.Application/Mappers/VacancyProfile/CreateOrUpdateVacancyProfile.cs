using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;
using EmploymentSystem.Domain.Entities;

namespace EmploymentSystem.Application.Mappers.VacancyProfile
{
    public class CreateOrUpdateVacancyProfile : Profiles
    {
        public CreateOrUpdateVacancyProfile()
        {
            CreateMap<CreateVacancyRequestDto, Vacancy>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());


            CreateMap<UpdateVacancyRequestDto, Vacancy>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreationDate, opt => opt.Ignore());


        }


    }
}
