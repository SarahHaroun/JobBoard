using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobBoard.Domain.DTO.CategoryDto;
using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category , categoryNameDto>()
               .ForMember(des => des.CatName , op => op.MapFrom(src => src.CategoryName));


            CreateMap<Category, CategoryDto>()
              .ForMember(des => des.CategoryName, op => op.MapFrom(src => src.CategoryName))
              .ForMember(des => des.CategoryDescription, op => op.MapFrom(src => src.CategoryDescription)) .ForMember(des => des.CategoryDescription, op => op.MapFrom(src => src.CategoryDescription))
              .ReverseMap();

        }
    }
}
