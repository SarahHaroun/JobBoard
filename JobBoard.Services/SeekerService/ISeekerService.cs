using JobBoard.Domain.DTO.SeekerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.SeekerService
{
    public interface ISeekerService
    {
        Task<List<SeekerProfileDto>> GetAllAsync();
        Task<SeekerProfileDto> GetByUserIdAsync(string userId);
        //Task<SeekerProfileResultDto> Create(SeekerProfileDto seekerProfile);
        Task<bool> UpdateAsync(string userId, SeekerProfileUpdateDto model);
		Task<(string? CvUrl, string? ProfileImageUrl)> UploadFilesAsync(string userId, SeekerFileUploadDto dto);
		Task<bool> DeleteAsync(int Id);
     
    }
}
