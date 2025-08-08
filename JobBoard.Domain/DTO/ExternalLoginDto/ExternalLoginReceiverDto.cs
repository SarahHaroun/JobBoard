using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.ExternalLoginDto
{
    public class ExternalLoginReceiverDto
    {
        public string IdToken { get; set; } = string.Empty;
        public string RoleFromClient { get; set; } = string.Empty;
    }
}
