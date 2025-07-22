using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Domain.Entities
{
    public enum UserType
    {
        Employer,Seeker
    }
    public class UserApplication : IdentityUser
    {
        public UserType User_Type { get; set; }

    }
}
