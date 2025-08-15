using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Attributes
{
	public class AllowedExtensionsAttribute : ValidationAttribute
	{
		private readonly string[] _extensions;

		public AllowedExtensionsAttribute(params string[] extensions)
		{
			_extensions = extensions;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is IFormFile file)
			{
				var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
				var allowedExtensions = _extensions.Select(x => x.ToLowerInvariant()).ToArray();

				if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension.TrimStart('.')))
				{
					return new ValidationResult(ErrorMessage ?? $"Only {string.Join(", ", _extensions)} files are allowed.");
				}
			}

			return ValidationResult.Success;
		}
	}
}
