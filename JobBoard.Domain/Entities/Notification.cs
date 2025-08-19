using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public string? Link { get; set; } // Optional link for more details


        /*-------------------------------------Application user-----------------------------*/
        // Foreign key to the user
        public string UserId { get; set; }
        
        // Navigation property to the user
        public ApplicationUser User { get; set; }
        // Constructor to initialize CreatedAt
        public Notification()
        {
            CreatedAt = DateTime.UtcNow;
            IsRead = false;
        }
    }
}
