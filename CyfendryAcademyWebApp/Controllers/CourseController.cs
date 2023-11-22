using CyfendryAcademyWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CyfendryAcademyWebApp.Controllers
{
    public class CourseController : Controller
    {
        private CourseService _courseService;
        private CoursePlanService _coursePlanService;
        public CourseController()
        {
            _courseService = new CourseService();
            _coursePlanService = new CoursePlanService();
        }

        [HttpGet]
        public async Task<ActionResult> GetAllCourses()
        {
            var coursesResponse = await _courseService.GetAllCoursesAsync();

            return Json(coursesResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> AddStripeCourses()
        {
            var coursesResponse = await _courseService.AddStripeProductId();

            return Json(coursesResponse, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<ActionResult> AddStripeCoursePlans()
        {
            var coursesResponse = await _coursePlanService.AddStripePriceId();

            return Json(coursesResponse, JsonRequestBehavior.AllowGet);
        }
    }
}