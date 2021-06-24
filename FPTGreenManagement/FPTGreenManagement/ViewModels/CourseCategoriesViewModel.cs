﻿using FPTGreenManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPTGreenManagement.ViewModels
{
    public class CourseCategoriesViewModel
    {
        public Course Course { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}