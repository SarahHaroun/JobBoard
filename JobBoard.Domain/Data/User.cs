using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace JobBoard.Domain.Data
{
    public enum UserType
    {
        Employer,Seeker
    }
    public class User :IdentityUser
    {
        public UserType User_Type { get; set; }

    }
}
