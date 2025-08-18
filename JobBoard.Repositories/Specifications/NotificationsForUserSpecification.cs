using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities;

namespace JobBoard.Repositories.Specifications
{
    public class NotificationsForUserSpecification :BaseSpecifications<Notification>
    {
        public NotificationsForUserSpecification(string userId)
           : base(n => n.UserId == userId)
        {
            AddOrderByDesc(n => n.CreatedAt);
        }
    }
}
