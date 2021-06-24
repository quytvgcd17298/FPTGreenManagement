using FPTGreenManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTGreenManagement.ViewModels
{
    public class TrainerCoursesViewModel
    {
        public TrainerCourse TrainerCourse { get; set; }
        public IEnumerable<ApplicationUser> Trainers { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}