using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.JobDto
{
    public class JobDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Salary { get; set; }
        public string WorkplaceType { get; set; }
        public string JobType { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? EducationLevel { get; set; }
        public int? MinTeamSize { get; set; }
        public int? MaxTeamSize { get; set; }
        public string? ExperienceLevel { get; set; }
        public string? Requirements { get; set; }
        public bool IsActive { get; set; }

        // Employer info
        public int EmployerId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string Website { get; set; }

        // Skills
        public List<string> Skills { get; set; }

        // Categories
        public List<string> Categories { get; set; }
    }
}
