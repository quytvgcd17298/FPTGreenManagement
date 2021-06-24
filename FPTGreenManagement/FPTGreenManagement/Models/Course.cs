using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPTGreenManagement.Models
{
    public class Course
    {
      
            [Key]
            public int Id { get; set; }
            [Required]
            public string CourseName { get; set; }
            [Required]
            public string Description { get; set; }

            [Required]
            [ForeignKey("Category")]
            public int CategoryId { get; set; }
            public Category Category { get; set; }

            [ForeignKey("User")]
            public string UserId { get; set; }
            public ApplicationUser User { get; set; }
        }
    }