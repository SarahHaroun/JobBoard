using AutoMapper;
using JobBoard.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllCategories()
        {
            var result = await categoryService.GetAllCategoryAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllJobsByCategorieIdAsync(int id)
        {
            var jobs = await categoryService.GetJobsByCategoryIdAsync(id);
            return Ok(jobs);
        }

    }
}
