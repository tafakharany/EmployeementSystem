using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Domain.Entities
{
    public class ApplicantApplications
    {
        public ApplicantApplications(int applicantId, int applicationId)
        {
            ApplicantId = applicantId;
            ApplicationId = applicationId;
            LastAppliedDateTime = DateTime.Now;
        }
        public User Applicant { get; set; }
        public Application Application { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicantId { get; set; }
        public DateTime LastAppliedDateTime { get; set; }
    }
}
