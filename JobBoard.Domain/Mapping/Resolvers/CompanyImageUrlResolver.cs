using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping.Resolvers
{
	public class CompanyImageUrlResolver : IValueResolver<EmployerSeedDto, EmployerProfile, string>
	{
		private readonly IConfiguration _configuration; 

		public CompanyImageUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration; 
		}

		public string Resolve(EmployerSeedDto source, EmployerProfile destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.CompanyImage))
				return $"{_configuration["ApiBaseUrl"]}/{source.CompanyImage}";
			else
				return string.Empty;
		}
	}
}
