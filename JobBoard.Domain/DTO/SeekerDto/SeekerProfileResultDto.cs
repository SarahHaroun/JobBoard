using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.SeekerDto
{
    public class SeekerProfileResultDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public SeekerProfileDto? Data { get; set; }
    }
}
