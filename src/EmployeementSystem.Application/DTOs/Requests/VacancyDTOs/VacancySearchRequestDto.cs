using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Requests.VacancyDTOs
{
    public class VacancySearchRequestDto
    {
        public string? Title { get; set; }
        public string? VacancyNumber { get; set; }
    }
}
