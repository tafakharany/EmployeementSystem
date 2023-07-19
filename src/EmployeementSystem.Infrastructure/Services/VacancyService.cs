using AutoMapper;
using EmploymentSystem.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmploymentSystem.Infrastructure.Services;

public class VacancyService : BaseService<VacancyService>, IVacancyService
{
    public VacancyService(IMapper mapper, ILogger<VacancyService> logger, IApplicationDbContext context) : base(mapper, logger, context)
    {
    }

    public async Task UpdateExpiredVacancies()
    {
        try
        {
            var dayToExpireAt = DateTime.Today;
            var listOfVacanciesAsync = await Context.Vacancies.Where(x => x.ExpiryDate <= dayToExpireAt)
                 .ToListAsync();

            listOfVacanciesAsync.ForEach(x => x.IsActive = false);
            Context.Vacancies.UpdateRange(listOfVacanciesAsync);
            await Context.SaveChangesAsync(default);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message, ex.StackTrace);
        }

    }
}