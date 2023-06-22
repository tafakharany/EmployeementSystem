using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmploymentSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@" Create View View_Applicants_List
                                        As
                                        Select v.Id                                        [Vacancy_Id],
                                            v.Title,
                                            CONCAT(ANU.FirstName, ' ', ANU.LastName) as [ApplicantName],
                                            ANU.Email                                   [ApplicantEmail],
                                            A.ApplicationDate                           [AppliedAt],
                                            ANU.Id                                      [Applicant_Id]
                                        From Vacancies v
                                        Join Applications A on v.Id = A.VacancyId
                                        Join ApplicantApplications AA on A.Id = AA.ApplicantId
                                        Join AspNetUsers ANU on AA.ApplicantId = ANU.Id");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "61922c2a-14d1-41f1-a20a-e9eab3da8e57");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e5770fb7-e7fc-43ff-bfc2-fd85ff24c187");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "26156c67-a93b-4de1-b2ae-0a174029595b");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Drop View View_Applicants_List");
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "230163a3-115b-4c9f-a6a4-9c49f07cd83f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "640d0d04-d8c5-405f-8ae6-845359b1a682");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "fe6492d7-1fc3-4907-a05d-05433e69ccd8");
        }
    }
}
