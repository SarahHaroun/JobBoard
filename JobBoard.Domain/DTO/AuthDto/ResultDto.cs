using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AuthDto
{
    public class ResultDto
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public ResultDto(bool succeeded, string message )
        {
            this.Succeeded = succeeded;
            this.Message = message;
        }
      
    }
    public class ResultLoginDto
    {

        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? Role { get; set; }

    }
}
