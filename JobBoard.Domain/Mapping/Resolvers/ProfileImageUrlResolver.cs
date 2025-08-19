using AutoMapper;
using JobBoard.Domain.DTO.EmployerDto;
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
    public class ProfileImageUrlResolver : IValueResolver<SeekerSeedDto, SeekerProfile, string>
    {
        private readonly IConfiguration _configuration;

        public ProfileImageUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(SeekerSeedDto source, SeekerProfile destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfileImageUrl))
                return $"{_configuration["ApiBaseUrl"]}/{source.ProfileImageUrl}";
            else
                return string.Empty;
        }
    }
}

