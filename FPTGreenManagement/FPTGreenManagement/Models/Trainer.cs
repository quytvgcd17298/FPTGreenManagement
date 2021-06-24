using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPTGreenManagement.Models
{
    public class Trainer
    {
        [Key]
        [ForeignKey("User")]
            public string Id { get; set; }
            public ApplicationUser User { get; set; }

        public string TrainerName { get; set; }
        public string WorkingPlace { get; set; }
        public string Telephone { get; set; }    }
}