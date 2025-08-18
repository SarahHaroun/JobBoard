using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobBoard.Domain.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Domain.Entities
{
    
    public class ApplicationUser : IdentityUser
    {
        public UserType User_Type { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public EmployerProfile employerProfile { get; set; }
        public SeekerProfile seekerProfile { get; set; }
        public List<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
