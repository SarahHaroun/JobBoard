using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto.SeekerSeedDto
{
    public class SeekerExperienceSeedDto
    {
        public string? JobTitle { get; set; }
        public string? CompanyName { get; set; }
        public string? Location { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Description { get; set; }
    }
}
