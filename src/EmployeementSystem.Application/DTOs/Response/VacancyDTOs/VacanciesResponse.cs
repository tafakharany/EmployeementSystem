using EmploymentSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Response.VacancyDTOs
{
    public class VacanciesResponseDto : ResponseDto
    {
        public IEnumerable<VacancyResponseDto> Vacancies { get; set; }
    }
}
