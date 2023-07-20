using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Domain.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmploymentSystem.Application.Contracts;

public interface IApplicationDbContext
{
    Task<IDbContextTransaction> Transaction { get; }
    DbSet<User> Users { get; }
    DbSet<ViewApplicantsList> ApplicantsPerVacancy { get; }
    //DbSet<ApplicantLastActivity> ApplicantsActivities { get; }
    DbSet<ApplicantApplications> ApplicantApplications { get; }
    DbSet<Vacancy> Vacancies { get; }
    DbSet<Domain.Entities.Application> Applications { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}


