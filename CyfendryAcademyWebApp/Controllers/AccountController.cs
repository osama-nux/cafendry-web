using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Models.RequestModels;
using CyfendryAcademyWebApp.Models.ViewModels;
using CyfendryAcademyWebApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CyfendryAcademyWebApp.Controllers
{
    public class AccountController : Controller
    {
        private AccountService _accountService;
        private PaymentService _paymentService;
        public AccountController()
        {
            _accountService = new AccountService();
            _paymentService = new PaymentService();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterRequestModel registerRequestModel)
        {
            if (ModelState.IsValid)
            {
                var registrationResponse = await _accountService.RegisterAsync(registerRequestModel);

                return Json(registrationResponse);
            }
            else
            {
                var errors = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return Json(new GenericResponseModel<RegisterRequestModel> { errorMessage = "form invalid!", status = false, userMessage = errors });
            }
        }
        [HttpGet]
        public ActionResult SubscriptionRegistered()
        {
            var session = _paymentService.GetStripeSessionBySessionId(TempData["stripe-session"].ToString());

            if (session.PaymentStatus == "paid")
            {

            }

            return View();
        }
        [HttpGet]
        public async Task<ActionResult> Registered()
        {
            var stripeSession = TempData["stripe-session"];

            if (stripeSession != null)
            {
                var session = _paymentService.GetStripeSessionBySessionId(stripeSession.ToString());

                if (session.Status == "complete" && session.PaymentStatus == "paid")
                {
                    await _paymentService.UpdatePaymentBySessionIdAsync(stripeSession.ToString(), session.Mode);
                }
            }
            
            return View();
        }
    }
}