using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Data
{
    public enum ExperienceLevel
    {
        EntryLevel,MidLevel,Experienced
    }
    public class SeekerProfile 
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        public string? Address {  get; set; }
        public string CV_Url { get; set; }
        public List<Application>? UserApplications { get; set; }

        public List<Skill>? Skills { get; set; }
        public ExperienceLevel Experience_Level { get; set; }

        public User User { get; set; }
    }
}
