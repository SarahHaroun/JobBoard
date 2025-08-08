using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AuthDto
{
    public class ChangePassDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Old password is required")]
        [StringLength(100, ErrorMessage = "Old password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, ErrorMessage = "New password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is required")]
        [StringLength(100, ErrorMessage = "Confirm new password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm new password does not match new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
