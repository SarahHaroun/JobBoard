using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AuthDto
{
    public class LoginDto
    {

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName must be between 3 and 50 characters", MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]       
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
