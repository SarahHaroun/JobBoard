using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class SeekerExperience
    {
        public int Id { get; set; }
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }

        /*------------------------SeekerProfile--------------------------*/

        [ForeignKey("SeekerProfile")]
        public int SeekerProfileId { get; set; }
        public SeekerProfile SeekerProfile { get; set; }


       
    }
}
