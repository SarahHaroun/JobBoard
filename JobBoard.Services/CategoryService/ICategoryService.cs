using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.DTO.CategoryDto;
using JobBoard.Domain.DTO.JobDto;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Services.CategoryService
{
    public interface ICategoryService
    {
        public Task<List<CategoryDto>> GetAllCategoryAsync();
        public Task<categoryNameDto> GetAllCategoryNameAsync();
        public Task<IEnumerable<JobDto>> GetJobsByCategoryIdAsync(int categoryId);



    }
}
