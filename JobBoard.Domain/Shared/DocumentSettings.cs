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
	public static class DocumentSettings
	{
		// Upload File
		public static async Task<string> UploadFileAsync(IFormFile file, string folderName, IWebHostEnvironment env, IConfiguration configuration)
		{
			if (file == null || file.Length == 0)
				return null;

			var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
			var folderPath = Path.Combine(env.WebRootPath, folderName);

			// Generate folder if it doesn't exist
			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			var filePath = Path.Combine(folderPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return $"{configuration["ApiBaseUrl"]}/{folderName}/{fileName}";
		}

		// Delet File
		public static bool DeleteFile(string fileUrl, string folderName, IWebHostEnvironment env)
		{
			if (string.IsNullOrEmpty(fileUrl))
				return false;

			// اExtract file name from url
			var fileName = Path.GetFileName(new Uri(fileUrl).LocalPath);
			var filePath = Path.Combine(env.WebRootPath, folderName, fileName);

			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				return true;
			}

			return false;
		}
	}

}
