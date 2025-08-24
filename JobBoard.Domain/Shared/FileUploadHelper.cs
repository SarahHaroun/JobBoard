using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Shared
{
	public static class FileUploadHelper
	{
		// ===================== Handle generic file upload ====================
		public static async Task<string?> HandleFileUploadAsync(
			IFormFile? file,
			string? existingFileUrl,
			string folderPath,
			IWebHostEnvironment env,
			IConfiguration configuration,
			bool removeFile = false,
			string? defaultFileUrl = null
		)
		{
			// If the user chose to remove the file
			if (removeFile)
			{
				if (!string.IsNullOrEmpty(existingFileUrl) && !IsDefaultImage(existingFileUrl, defaultFileUrl))
				{
					DocumentSettings.DeleteFile(existingFileUrl, folderPath, env);
				}
				return defaultFileUrl;
			}

			// If the user uploaded a new file
			if (file != null && file.Length > 0)
			{
				if (!string.IsNullOrEmpty(existingFileUrl) && !IsDefaultImage(existingFileUrl, defaultFileUrl))
				{
					DocumentSettings.DeleteFile(existingFileUrl, folderPath, env);
				}
				var uploadedUrl = await DocumentSettings.UploadFileAsync(file, folderPath, env, configuration);
				return uploadedUrl;
			}

			// Return existing or default if nothing changed
			return existingFileUrl ?? defaultFileUrl;
		}

		// ===================== Helper to check if image is default ====================
		public static bool IsDefaultImage(string? imageUrl, string? defaultImageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl) || string.IsNullOrEmpty(defaultImageUrl))
				return false;

			if (imageUrl.Equals(defaultImageUrl, StringComparison.OrdinalIgnoreCase))
				return true;

			// Check common default filenames
			if (imageUrl.EndsWith("/images/profilepic/user.jpg", StringComparison.OrdinalIgnoreCase) ||
				imageUrl.Contains("user.jpg", StringComparison.OrdinalIgnoreCase))
				return true;

			if (imageUrl.EndsWith("/images/companies/default.jpg", StringComparison.OrdinalIgnoreCase) ||
				imageUrl.Contains("default.jpg", StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}
	}
}
