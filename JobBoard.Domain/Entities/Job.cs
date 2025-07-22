using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public enum LocationJob
    {
        OnSite,Remote,Hyprid
    }
    public enum JobTime
    {
        FullTime,PartTime
    }
    public class Job
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal? Salary { get; set; }
        public LocationJob Job_Location { get; set; }
        public JobTime Job_Time { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpireDate { get; set; }

        /*------------------------Category--------------------------*/

        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        /*------------------------Applications--------------------------*/
        public List<Application>? JobApplications { get; set; }

        /*------------------------Employer--------------------------*/
        [ForeignKey("EmployerProfile")]
        public int EmployerId { get; set; }
        public EmployerProfile Employer { get; set; }

        /*------------------------Skills--------------------------*/
        public List<Skill>? Skills { get; set; }
    }
}
