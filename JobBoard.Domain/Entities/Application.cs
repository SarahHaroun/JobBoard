using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
   
    public class Application
    {
        public int Id { get; set; }
        public string ResumeUrl { get; set; }
        public DateTime AppliedDate { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        /*------------------------job--------------------------*/
        [ForeignKey("Job")]
        public int JobId { get; set; }
        public Job Job { get; set; }

        /*------------------------SeekerProfile--------------------------*/

        [ForeignKey("SeekerProfile")]
        public int ApplicantId { get; set; } // refers to seeker
        public SeekerProfile Applicant { get; set; }
    }
}
