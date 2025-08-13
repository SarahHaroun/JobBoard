using AutoMapper;
using AutoMapper.Execution;
using JobBoard.Domain.DTO.SeekerDto;
using JobBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Mapping.Resolvers
{
	public class SeekerCvUrlResolver : IValueResolver<SeekerSeedDto, SeekerProfile, string>
	{
		private readonly IConfiguration _configuration;

		public SeekerCvUrlResolver(IConfiguration configuration)
		{
			_configuration = configuration;
		}
		public string Resolve(SeekerSeedDto source, SeekerProfile destination, string destMember, ResolutionContext context)
		{
			if(!string.IsNullOrEmpty(source.CV_Url))
				return $"{_configuration["ApiBaseUrl"]}/{source.CV_Url}";
			else
				return string.Empty;
		}
	}
}
