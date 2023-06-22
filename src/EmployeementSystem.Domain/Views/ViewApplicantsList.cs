using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Domain.Views;

public class ViewApplicantsList
{
    [Column("Vacancy_Id")]
    public int VacancyId { get; set; }
    [Column("Applicant_Id")]
    public int ApplicantId { get; set; }
    public string? ApplicantName { get; set; }
    public string? ApplicantEmail { get; set; }
    public DateTime AppliedAt { get; set; }
    public string? Title { get; set; }
}