using EmploymentSystem.Domain.lookups;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace EmploymentSystem.Domain.Entities;

[Table(name: "User")]
public sealed class User : IdentityUser<int>
{
    public string MobileNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<ApplicantApplications> Applications { get; set; }
    public ICollection<Vacancy> Vacancies { get; set; }
}
