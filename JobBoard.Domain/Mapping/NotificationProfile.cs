using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobBoard.Domain.DTO.NotificationsDto;
using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.Link, opt => opt.MapFrom(src => src.Link ?? string.Empty));
            CreateMap<Notification, NotificationDto>()
                .ReverseMap();
        }
    }
    
}
