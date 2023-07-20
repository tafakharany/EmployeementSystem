namespace EmploymentSystem.Domain.Entities;

public sealed class Application : BaseDbEntity
{
    public Application(int vacancyId)
    {
        VacancyId = vacancyId;
        ApplicationDate = DateTime.Now;
    }
    public int VacancyId { get; set; }
    public Vacancy Vacancy { get; set; }
    public DateTime ApplicationDate { get; set; }
    public ICollection<ApplicantApplications> Applicants { get; set; }
}
