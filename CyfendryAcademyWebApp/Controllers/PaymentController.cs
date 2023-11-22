using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Models.RequestModels;
using CyfendryAcademyWebApp.Models.ViewModels;
using CyfendryAcademyWebApp.Services;
//using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CyfendryAcademyWebApp.Controllers
{
    public class PaymentController : Controller
    {
        private PaymentService _paymentService;
        public PaymentController()
        {
            _paymentService = new PaymentService();
        }

        [HttpGet]
        public async Task<ActionResult> GoToPaymentCheckout(long userCourseId)
        {
            var session = await _paymentService.GetStripeCheckoutForPayment(userCourseId);

            if (session != null)
            {
                var paymentSessionCreateResponse = await _paymentService.CreatePaymentSession(userCourseId, null, session.Id);

                if (paymentSessionCreateResponse.status)
                {
                    TempData["stripe-session"] = session.Id;

                    return RedirectPermanent(session.Url);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<ActionResult> GoToSubscriptionCheckout(long userCourseId, int coursePlanId)
        {
            var session = await _paymentService.GetStripeCheckoutForSubscription(userCourseId, coursePlanId);

            if (session != null)
            {
                var paymentSessionCreateResponse = await _paymentService.CreatePaymentSession(userCourseId, coursePlanId, session.Id);

                if (paymentSessionCreateResponse.status)
                {
                    TempData["stripe-session"] = session.Id;

                    return RedirectPermanent(session.Url);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Failed()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> CardPayment(CardDetailsRequestModel cardDetailsRequestModel)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (ModelState.IsValid)
            {
                return Json(await _paymentService.CreateStripePayment(cardDetailsRequestModel));
            }
            else
            {
                return Json(new GenericResponseModel<PaymentVM> { errorMessage = "form invalid!", status = false, userMessage = "Please fill card details carefully!" });
            }
        }
        [HttpPost]
        public async Task<ActionResult> Charge(FormCollection form)
        {
            string token = form["stripeToken"];
            string email = form["stripeEmail"];
            string userCourseId = form["userCourseId"];
            string coursePlanId = form["coursePlanId"];
            string isFullPayment = form["isFullPayment"];

            CardDetailsRequestModel cardDetailsRequestModel = new CardDetailsRequestModel
            {
                StripeToken = token,
                IsFullPayment = Convert.ToBoolean(isFullPayment),
                UserCourseId = Convert.ToInt64(userCourseId),
                CoursePlanId = Convert.ToInt64(coursePlanId)
            };

            return Json(await _paymentService.CreateStripePayment(cardDetailsRequestModel));
        }
    }
}