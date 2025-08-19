using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities;

namespace JobBoard.Repositories.Specifications
{
    public class JobsWithDetailsSpec : BaseSpecifications<Job>
    {
        public JobsWithDetailsSpec() {
            AddIncludes(j => j.Employer);
            AddIncludes(j => j.Skills);
            AddIncludes(j => j.Categories);
        }
    }
}
