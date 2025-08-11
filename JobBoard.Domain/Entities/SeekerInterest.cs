using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class SeekerInterest
    {
        public int Id { get; set; }
        public string? InterestName { get; set; }

        /*------------------------SeekerProfile--------------------------*/
        [ForeignKey("SeekerProfile")]
        public int SeekerProfileId { get; set; }
        public SeekerProfile SeekerProfile { get; set; }
    }
}
