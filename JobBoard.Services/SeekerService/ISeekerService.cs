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
        Task<List<SeekerProfileDto>> GetAll();
        Task<SeekerProfileDto> GetByUserId(string userId);
        Task<SeekerProfileResultDto> Create(SeekerProfileDto seekerProfile);
        Task<SeekerProfileResultDto> Update(int id, SeekerProfileDto seekerProfile);
        Task<bool> DeleteById(int id);
     
    }
}
