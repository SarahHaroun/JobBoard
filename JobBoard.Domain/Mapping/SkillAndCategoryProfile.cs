using AutoMapper;
using JobBoard.Domain.DTO.CategoryDto;
using JobBoard.Domain.DTO.SkillAndCategoryDto;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
    public class SkillAndCategoryProfile : Profile
    {
		public SkillAndCategoryProfile()
		{
			CreateMap<Category, CategoryDto>();
			CreateMap<Skill, SkillDto>();
		}
	}
}
