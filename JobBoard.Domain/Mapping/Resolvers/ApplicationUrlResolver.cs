using AutoMapper;
using JobBoard.Domain.DTO.ApplicationDto;
using JobBoard.Domain.DTO.SeekerDto.SeekerSeedDto;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping.Resolvers
{
	public class ApplicationUrlResolver : IValueResolver<(ApplicationSeedDto userDto, ApplicationDetailDto appDto), Application, string>
	{
		private readonly IConfiguration _configuration;

		public ApplicationUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string Resolve((ApplicationSeedDto userDto, ApplicationDetailDto appDto) source, Application destination, string destMember, ResolutionContext context)
		{
			if (!string.IsNullOrEmpty(source.userDto.ResumeUrl))
				return $"{_configuration["ApiBaseUrl"]}/{source.userDto.ResumeUrl}";
			else
				return string.Empty;
		}
	}
}
