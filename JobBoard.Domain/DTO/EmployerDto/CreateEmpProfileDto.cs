using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.EmployerDto
{
    public class CreateEmpProfileDto
    {
        [Required(ErrorMessage = "Company name is required")]
        [MaxLength(100)]
        public string CompanyName { get; set; }

        [MaxLength(150)]
        public string? CompanyLocation { get; set; }

        public string? UserId { get; set; }

    }
}
