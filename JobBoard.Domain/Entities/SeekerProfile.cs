using JobBoard.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities
{
    public class SeekerProfile 
    {
        public int Id { get; set; }
       
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public string? Address {  get; set; }
        public string CV_Url { get; set; }
        public ExperienceLevel Experience_Level { get; set; }

        /*------------------------user--------------------------*/

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }

        /*------------------------Application--------------------------*/
        public List<Application>? UserApplications { get; set; }

		/*------------------------Skills--------------------------*/
		public ICollection<Skill> Skills { get; set; } = new List<Skill>();

	}
}
