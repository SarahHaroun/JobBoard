using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public class EmployerProfile 
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string CompanyName { get; set; }
        
        public string? CompanyLocation { get; set; }
        public List<Job>? PostedJobs { get; set; }

        public User User { get; set; }
    }
}
