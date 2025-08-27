using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoolProof.Core;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Enums;

namespace JobBoard.Domain.DTO.ExternalLoginDto
{
	public class ExternalLoginReceiverDto
	{
		public string IdToken { get; set; } = string.Empty;
		public string RoleFromClient { get; set; } = string.Empty;

		/*------------------------Employer--------------------------*/

		[RequiredIf("RoleFromClient", UserType.Employer, ErrorMessage = "CompanyName is required for Employer")]
		[StringLength(100, ErrorMessage = "CompanyName must be between 3 and 100 characters", MinimumLength = 2)]
		public string? CompanyName { get; set; }  // Only for Employer


		[RequiredIf("RoleFromClient", UserType.Employer, ErrorMessage = "CompanyLocation is required for Employer")]
		[StringLength(200, ErrorMessage = "CompanyLocation must be between 3 and 200 characters", MinimumLength = 3)]
		public string? CompanyLocation { get; set; }  // Only for Employer
	}
}
