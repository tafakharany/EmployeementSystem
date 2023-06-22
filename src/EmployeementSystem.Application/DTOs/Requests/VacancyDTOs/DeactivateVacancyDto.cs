using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Requests.VacancyDTOs
{
    public class DeactivateVacancyDto
    {
        public int VacancyId { get; set; }
        public int EmployerId { get; set; }
    }
}
