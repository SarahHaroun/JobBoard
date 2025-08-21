using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto.SeekerSeedDto
{
    public class SeekerEducationSeedDto
    {
        public string? Major { get; set; }
        public string? Faculty { get; set; }
        public string? University { get; set; }
        public DateTime? Date { get; set; }
        public string? Location { get; set; }
        public double? GPA { get; set; }
        public EducationLevel? EducationLevel { get; set; }
    }
}
