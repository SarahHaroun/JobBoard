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
        //Task<SeekerProfileResultDto> Create(SeekerProfileDto seekerProfile);
        Task<bool> Update(int id, SeekerProfileUpdateDto model);
        Task<bool> DeleteById(int id);
     
    }
}
