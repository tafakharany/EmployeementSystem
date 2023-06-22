using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmploymentSystem.Application.DTOs.Response.VacancyDTOs;
using EmploymentSystem.Domain.Entities;

namespace EmploymentSystem.Application.Mappers.VacancyProfile
{
    public class VacancySearchResultProfile:Profiles
    {
        public VacancySearchResultProfile()
        {
            CreateMap<Vacancy, VacancyResponseDto>().ReverseMap();
        }
    }
}
