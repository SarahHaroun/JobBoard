using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto
{
    public class SeekerSeedDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CV_Url { get; set; }
        public ExperienceLevel Experience_Level { get; set; }
        public Gender Gender { get; set; }
        public List<int> Skills { get; set; }
        public string UserEmail { get; set; }

    }
}
