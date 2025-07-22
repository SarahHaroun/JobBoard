using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class Category
    {
		public int Id { get; set; }
		public string CategoryName { get; set; }
		public string? CategoryDescription { get; set; }

		/*------------------------job--------------------------*/
		public ICollection<Job> Jobs { get; set; }
	}
}
