using JobBoard.Domain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AuthDto
{
    public class RegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType user_type { get; set; }  // UserType can be Employer or Seeker


    }
}
