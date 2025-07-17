using JobBoard.Domain.DTO;
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
        Task<EmpProfileDto> GetById(int id);
        Task<string> Create(CreateEmpProfileDto empProfile);
        Task<bool> Update(int id, UpdateEmpProfileDto empProfile);

        Task<bool> DeleteById(int id);
        Task<EmpProfileDto?> GetByUserIdAsync(string userId);

    }
}
