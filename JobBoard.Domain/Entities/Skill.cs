using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Skill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }

		/*------------------------job--------------------------*/

		public ICollection<Job> Jobs { get; set; }
		public ICollection<SeekerProfile> Seekers { get; set; }
	}
}
