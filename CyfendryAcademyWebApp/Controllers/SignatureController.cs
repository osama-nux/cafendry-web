using CyfendryAcademyWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CyfendryAcademyWebApp.Controllers
{
    public class SignatureController : Controller
    {
        private UserCourseService _userCourseService;
        private UserCourseSignatureService _courseSignatureService;
        public SignatureController()
        {
            _userCourseService = new UserCourseService();
            _courseSignatureService = new UserCourseSignatureService();
        }
        [HttpGet]
        public async Task<ActionResult> Upload(string customerId, string productId)
        {
            var userCourse = await _userCourseService.GetByCustomerIdAndProductIdAsync(customerId, productId);

            if (userCourse.status)
            {
                TempData["user-course"] = userCourse.model.Id;
            }
            else
            {

            }

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Upload(HttpPostedFileBase file)
        {
            long userCourse = Convert.ToInt64(TempData["user-course"]);
            if (userCourse > 0)
            {
                return Json(await _courseSignatureService.AddUserCourseSignatureAsync(userCourse, Server.MapPath("~/Assets/Users/Signatures"), file));
            }
            else
            {
                return Json(new { status = false, userMessage = "no course found!", errorMessage = "no course header found!" });
            }
        }
    }
}