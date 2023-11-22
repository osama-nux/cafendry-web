using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CyfendryAcademyWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Courses()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Coaching()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Blogs()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Questions()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Events()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ITAudits()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ITGRC()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ITPMT()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Cloud()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}