using System.Reflection;
using EmploymentSystem.Application.Contracts;
using EmploymentSystem.Domain.Entities;
using EmploymentSystem.Domain.Views;
using EmploymentSystem.Infrastructure.Utils;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmploymentSystem.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, int>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Sets

    public Task<IDbContextTransaction> Transaction => this.Database.BeginTransactionAsync();

    public override DbSet<User> Users => Set<User>();

    public DbSet<ViewApplicantsList> ApplicantsPerVacancy => Set<ViewApplicantsList>();

    public DbSet<ApplicantApplications> ApplicantApplications => Set<ApplicantApplications>();
    //public DbSet<ApplicantLastActivity> ApplicantsActivities => Set<ApplicantLastActivity>();

    //public DbSet<Applicant> Applicants => Set<Applicant>();
    //public DbSet<Employer> Employers => Set<Employer>();
    public DbSet<Vacancy> Vacancies => Set<Vacancy>();
    public DbSet<Domain.Entities.Application> Applications => Set<Domain.Entities.Application>();
    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Configure the User entity
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Vacancy>()
            .HasOne(v => v.Employer)
            .WithMany(e => e.Vacancies)
            .HasForeignKey(v => v.CreatedBy);

        modelBuilder.Entity<Domain.Entities.Application>()
            .HasOne(a => a.Vacancy)
            .WithMany(v => v.Applications)
            .HasForeignKey(a => a.VacancyId);

        modelBuilder.Entity<ApplicantApplications>().HasKey(a => new { a.ApplicantId, a.ApplicationId });

        modelBuilder.Entity<ApplicantApplications>()
            .HasOne(a => a.Applicant)
            .WithMany(a => a.Applications)
            .HasForeignKey(a => a.ApplicationId);

        modelBuilder.Entity<ApplicantApplications>()
            .HasOne(a => a.Application)
            .WithMany(a => a.Applicants)
            .HasForeignKey(a => a.ApplicantId);
        //modelBuilder.Entity<ApplicantLastActivity>()
        //    .HasNoKey()
        //    .ToView("Applicant_Last_Activity");

        modelBuilder.Entity<ViewApplicantsList>()
            .HasNoKey()
            .ToView("View_Applicants_List");


        new DbInitializer(modelBuilder).Seed();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
