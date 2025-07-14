using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public enum LocationJob
    {
        OnSite,Remote,Hyprid
    }
    public class Job
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal? Salary { get; set; }
        public LocationJob Location_Job { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int EmployerId { get; set; }
        public EmployerProfile Employer { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<Application> JobApplications { get; set; }
    }
}
