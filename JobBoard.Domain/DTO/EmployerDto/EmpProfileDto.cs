using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.EmployerDto
{
    public class EmpProfileDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }

        public string? CompanyLocation { get; set; }
        public string? UserId { get; set; }


    }
}
