using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public enum ExperienceLevel
    {
        EntryLevel,MidLevel,Experienced
    }
    public enum Gender
    {
        female,
        male,
        other
    }
    public class SeekerProfile 
    {
        public int Id { get; set; }
       
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public string? Address {  get; set; }
        public string CV_Url { get; set; }

        public ExperienceLevel Experience_Level { get; set; }

        public Gender Gender { get; set; }

        /*------------------------user--------------------------*/

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public UserApplication User { get; set; }

        /*------------------------Application--------------------------*/
        public List<Application>? UserApplications { get; set; }

        /*------------------------Skills--------------------------*/
        public List<Skill>? Skills { get; set; }
       
    }
}
