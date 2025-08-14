using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class SavedJob
    {
		public int Id { get; set; }
		public int JobId { get; set; }
		public Job Job { get; set; }
		public int SeekerId { get; set; }
		public SeekerProfile Seeker { get; set; }
		public DateTime SavedAt { get; set; } = DateTime.UtcNow;
	}
}
