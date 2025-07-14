using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public enum ApplicationStatus
    {
        Pending,Accepted,Rejected
    }
    public class Application
    {
        public int Id { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }
        public int ApplicantId { get; set; } // refers to seeker
        public SeekerProfile Applicant { get; set; }  
        public string ResumeUrl { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    }
}
