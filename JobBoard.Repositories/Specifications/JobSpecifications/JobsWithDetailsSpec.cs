using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities;

namespace JobBoard.Repositories.Specifications.JobSpecifications
{
    public class JobsWithDetailsSpec : BaseSpecifications<Job>
    {
        public JobsWithDetailsSpec():base(j => j.IsApproved) {
            AddIncludes(j => j.Employer);
            AddIncludes(j => j.Skills);
            AddIncludes(j => j.Categories);
        }
    }
}
