using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPTGreenManagement.Models
{
    public class TrainerCourse
    {
        [Key]
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int CourseId { get; set; }
        public ApplicationUser Trainer { get; set; }
        public Course Course { get; set; }
    }
}