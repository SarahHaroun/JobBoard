using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.DTO.AdminDto
{
    public class SeekerListDto
    {
		public int Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public List<string>? SkillName { get; set; } = new();

	}
}
