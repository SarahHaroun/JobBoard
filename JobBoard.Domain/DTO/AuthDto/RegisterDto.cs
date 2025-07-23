using JobBoard.Domain.Entities;
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
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(50, ErrorMessage = "UserName must be between 3 and 50 characters", MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "UserName can only contain letters and numbers")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,}$", 
        //    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "User type is required")]
        [EnumDataType(typeof(UserType), ErrorMessage = "Invalid user type")]
        public UserType user_type { get; set; }  // UserType can be Employer or Seeker

        /*------------------------Employer--------------------------*/

        [RequiredIf("user_type", UserType.Employer, ErrorMessage = "CompanyName is required for Employer")]
        [StringLength(100, ErrorMessage = "CompanyName must be between 3 and 100 characters", MinimumLength = 2)]
        public string? CompanyName { get; set; }  // Only for Employer


        [RequiredIf("user_type", UserType.Employer, ErrorMessage = "CompanyLocation is required for Employer")]
        [StringLength(200, ErrorMessage = "CompanyLocation must be between 3 and 200 characters", MinimumLength = 3)]
        public string? CompanyLocation { get; set; }  // Only for Employer




    }
}
