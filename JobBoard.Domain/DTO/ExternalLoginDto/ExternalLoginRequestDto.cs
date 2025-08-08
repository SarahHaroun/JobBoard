using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ExternalLoginDto
{
    public class ExternalLoginRequestDto
    {
        public string IdToken { get; set; }
        public string Role { get; set; }  // Seeker or Employer
    }
}
