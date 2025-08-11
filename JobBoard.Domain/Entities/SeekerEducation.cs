using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class SeekerEducation
    {
        public int Id { get; set; }
        public string? Major { get; set; }
        public string? Faculty { get; set; }
        public string? University { get; set; }
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public double? GPA { get; set; }
        public EducationLevel? EducationLevel { get; set; }

        /*------------------------SeekerProfile--------------------------*/
        [ForeignKey("SeekerProfile")]
        public int SeekerProfileId { get; set; }
        public SeekerProfile SeekerProfile { get; set; }
       

    }
}
