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
using JobBoard.Repositories.Specifications;

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
        public async Task<List<CategoryDto>> GetAllCategoryAsync(int? jobId)
        {
            var categories = await unitOfWork.Repository<Category>().GetAllAsync();
            var mapCategory = mapper.Map<List<CategoryDto>>(categories);
            return mapCategory;
        }
    }
}
