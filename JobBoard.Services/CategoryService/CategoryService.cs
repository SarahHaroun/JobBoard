using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobBoard.Domain.DTO.CategoryDto;
using JobBoard.Domain.DTO.JobDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;

namespace JobBoard.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IMapper mapper, IUnitOfWork unitOfWork )
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }
        public async Task<List<CategoryDto>> GetAllCategoryAsync()
        {
            var categories = await unitOfWork.Repository<Category>().GetAllAsync();
            var mapCategory = mapper.Map<List<CategoryDto>>(categories);
            return mapCategory;
        }

        public async Task<IEnumerable<JobDto>> GetJobsByCategoryIdAsync(int categoryId)
        {
            var jobs = await unitOfWork.Repository<Job>().GetAllAsync();

            // Filter jobs where this category exists
            var filteredJobs = jobs
                .Where(j => j.Categories.Any(c => c.Id == categoryId));

            var jobDtos = mapper.Map<IEnumerable<JobDto>>(filteredJobs);
            return jobDtos;
        }

        public Task<categoryNameDto> GetAllCategoryNameAsync()
        {
            throw new NotImplementedException();
        }

    }
}
