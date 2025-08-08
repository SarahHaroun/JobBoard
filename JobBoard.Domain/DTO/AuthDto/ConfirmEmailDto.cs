using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AuthDto
{
    public class ConfirmEmailDto
    {
        public string Email { get; set; }

        public string Token { get; set; }


    }
}
