using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.Models.ViewModels
{
    public class CoursePlanListVM
    {
        public int Id { get; set; }
        public Nullable<decimal> UpFrontPrice { get; set; }
        public Nullable<decimal> PlanPrice { get; set; }
        public string Interval { get; set; }
        public Nullable<int> IntervalValue { get; set; }
    }
}