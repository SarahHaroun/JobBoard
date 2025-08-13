using JobBoard.Domain.DTO.EmployerDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services.EmployerService
{
    public interface IEmployerService
    {
        Task<List<EmpProfileDto>> GetAll();
        Task<EmpProfileDto> GetByUserId(string userId);
        //Task<string> Create(EmpProfileUpdateDto empProfile);
        Task<bool> Update(int id, EmpProfileUpdateDto empProfile);
        Task<bool> DeleteById(int id);

    }
}
