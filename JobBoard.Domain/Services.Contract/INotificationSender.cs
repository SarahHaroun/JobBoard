using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Services.Contract
{
    public interface INotificationSender
    {
        public Task SendNotificationAsync(string userId, string message, string? link = null, int id = 0);
    }
}
