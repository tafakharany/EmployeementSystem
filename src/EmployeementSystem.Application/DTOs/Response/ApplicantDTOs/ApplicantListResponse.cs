using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Application.DTOs.Response.ApplicantDTOs
{
    public class ApplicantListResponseDTO : ResponseDto
    {
        public IEnumerable<ApplicantDto>? Applicants { get; set; }
    }
}
