using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			CreateMap<EmployerSeedDto, EmployerProfile>();

		}
	}
}
