using Newtonsoft.Json;

namespace EmploymentSystem.Application.DTOs.Requests.VacancyDTOs;

public class VacancyCreateOrUpdateRequestDto
{
    public string? Title { get; set; }
    public string? VacancyNumber { get; set; }
    public int MaxApplications { get; set; } = 20;
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }
    public DateTime ExpiryDate { get; set; }
}

public class CreateVacancyRequestDto : VacancyCreateOrUpdateRequestDto
{
    public int CreatedBy { get; set; }
    [JsonIgnore]
    public DateTime CreationDate { get; set; } = DateTime.Now;
}
public class UpdateVacancyRequestDto : VacancyCreateOrUpdateRequestDto
{
    [JsonIgnore]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public int UpdatedBy { get; set; }
}
