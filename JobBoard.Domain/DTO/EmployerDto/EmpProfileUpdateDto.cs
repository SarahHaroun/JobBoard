using JobBoard.Domain.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.EmployerDto
{
    public class EmpProfileUpdateDto
    {

        [Required(ErrorMessage = "Company name is required")]
        [StringLength(100, ErrorMessage = "Company name must be less than 100 characters")]
        public string CompanyName { get; set; }

        [StringLength(150, ErrorMessage = "Location must be less than 150 characters")]
        public string? CompanyLocation { get; set; }

		[DataType(DataType.Upload)]

		public bool RemoveCompanyImage { get; set; } = false;

		[AllowedExtensions("jpg", "jpeg", "png", "gif", "webp", ErrorMessage = "Only image files are allowed.")]
		public IFormFile? CompanyImage { get; set; }

        [Url(ErrorMessage = "Website must be a valid URL")]
        public string? Website { get; set; }

        [StringLength(50, ErrorMessage = "Industry must be less than 50 characters")]
        public string? Industry { get; set; }

        [StringLength(1000, ErrorMessage = "Description must be less than 1000 characters")]
        public string? CompanyDescription { get; set; }

        [StringLength(500, ErrorMessage = "Mission must be less than 500 characters")]
		public string? CompanyMission { get; set; }
		public string? EmployeeRange { get; set; }

		[Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
        public int? EstablishedYear { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
        public string? Phone { get; set; }
        public string? Password { get; set; }
    }
}
