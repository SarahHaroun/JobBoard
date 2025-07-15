using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public class Skill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }

        /*------------------------job--------------------------*/
        [ForeignKey("Job")]
        public int? JobId { get; set; }
        public Job Job { get; set; }


    }
}
