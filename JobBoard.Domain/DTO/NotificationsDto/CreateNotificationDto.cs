using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.NotificationsDto
{
    public class CreateNotificationDto
    {
        //public string UserId { get; set; }
        public string Message { get; set; }
        public string? Link { get; set; }
    }
}
