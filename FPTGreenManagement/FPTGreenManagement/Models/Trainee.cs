using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPTGreenManagement.Models
{
    public class Trainee
    {
        [Key]
        [ForeignKey("User")]
        public string Id { get; set; }
        public ApplicationUser User { get; set; }
        public string TraineeName { get; set; }
        [Range(1, 150, ErrorMessage = "Please enter Age value bigger than 0 and less than 150")]
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Education { get; set; }
        public string MainProgrammingLanguage { get; set; }
        public float TOEICscore { get; set; }
        public string ExperienceDetails { get; set; }
        public string Department { get; set; }
        public string Location { get; set; }
    }
}