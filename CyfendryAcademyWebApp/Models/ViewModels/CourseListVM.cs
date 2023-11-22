using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.Models.ViewModels
{
    public class CourseListVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Duration { get; set; }
        public List<CoursePlanListVM> CoursePlans { get; set; }
    }
}