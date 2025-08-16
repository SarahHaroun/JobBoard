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
		public static async Task<string> UploadFileAsync(IFormFile file, string folderPath, IWebHostEnvironment env, IConfiguration configuration)
		{
			var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
			var uploadPath = Path.Combine(env.WebRootPath, folderPath);

			if (!Directory.Exists(uploadPath))
				Directory.CreateDirectory(uploadPath);

			var filePath = Path.Combine(uploadPath, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}

			return $"{configuration["ApiBaseUrl"]}/{folderPath.Replace("\\", "/")}/{fileName}";
		}

		public static void DeleteFile(string fileUrl, string folderPath, IWebHostEnvironment env)
		{
			if (string.IsNullOrEmpty(fileUrl)) return;

			var fileName = Path.GetFileName(fileUrl);
			var fullPath = Path.Combine(env.WebRootPath, folderPath, fileName);

			if (File.Exists(fullPath))
				File.Delete(fullPath);
		}
	}

}
