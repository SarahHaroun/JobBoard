using JobBoard.Domain.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto
{
    public class SeekerFileUploadDto
    {
		[AllowedExtensions("jpg", "jpeg", "png", "gif", "webp", ErrorMessage = "Only image files are allowed.")]
		public IFormFile? ProfileImageUrl { get; set; }

		[AllowedExtensions("pdf", "doc", "docx", ErrorMessage = "Only PDF and Word documents are allowed for CV.")]
		public IFormFile? CV_Url { get; set; }
		public bool RemoveProfileImage { get; set; } = false;
		public bool RemoveCV { get; set; } = false;
	}
}
