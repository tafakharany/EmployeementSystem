namespace EmploymentSystem.Domain.Entities;

public sealed class Vacancy : BaseDbEntity
{
    public string VacancyNumber { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int MaxApplications { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
    public User Employer { get; set; }
    public ICollection<Application> Applications { get; set; }
}