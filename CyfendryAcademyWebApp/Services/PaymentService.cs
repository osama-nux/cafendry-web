using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Models.RequestModels;
using CyfendryAcademyWebApp.Models.ViewModels;
using CyfendryAcademyWebApp.PaymentGateways;
using CyfendryAcademyWebApp.Repositories;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;

namespace CyfendryAcademyWebApp.Services
{
    public class PaymentService
    {
        private IPaymentRepository _paymentRepository;
        private IUserCourseRepository _userCourseRepository;
        private IUserPaymentMethodRepository _userPaymentMethodRepository;
        private ISubscriptionRepository _subscriptionRepository;
        public PaymentService()
        {
            admin_cyfendrydbEntities admin_CyfendrydbEntities = new admin_cyfendrydbEntities();

            _paymentRepository = new PaymentRepository(admin_CyfendrydbEntities);
            _userCourseRepository = new UserCourseRepository(admin_CyfendrydbEntities);
            _userPaymentMethodRepository = new UserPaymentMethodRepository(admin_CyfendrydbEntities);
            _subscriptionRepository = new SubscriptionRepository(admin_CyfendrydbEntities);
        }

        public async Task<GenericResponseModel<Payment>> GetBySessionIdAsync(string sessionId)
        {
            var paymentResponse = await _paymentRepository.GetBySessionIdAsync(sessionId);

            if (paymentResponse != null)
            {
                return new GenericResponseModel<Payment> { model = paymentResponse, status = true, successMessage = "payment found!" };
            }
            else
            {
                return new GenericResponseModel<Payment> { errorMessage = "payment not found!", status = false, userMessage = "payment session not found! please try again later!" };
            }
        }
        public async Task<GenericResponseModel<Payment>> CreatePaymentSession(long userCourseId, int? coursePlanId, string sessionId)
        {
            Payment payment = new Payment
            {
                CoursePlanId = coursePlanId,
                IsPaid = false,
                Status = "Pending",
                StripeSessionId = sessionId,
                UserCourseId = userCourseId
            };

            bool paymentCreateResponse = await _paymentRepository.CreateAsync(payment);

            if (paymentCreateResponse)
            {
                return new GenericResponseModel<Payment> { model = payment, status = paymentCreateResponse, successMessage = "payment created!" };
            }
            else
            {
                return new GenericResponseModel<Payment> { errorMessage = "payment creation failed!", status = false, userMessage = "payment session create failed! please try again later!" };
            }
        }
        public async Task<Payment> UpdatePaymentBySessionIdAsync(string sessionId, string mode)
        {
            var stripeSession = GetStripeSessionBySessionId(sessionId);

            if (stripeSession != null)
            {
                var payment = await GetBySessionIdAsync(sessionId);

                if (payment.status)
                {
                    if (mode == "payment")
                    {
                        payment.model.Status = "Paid";
                        payment.model.IsPaid = true;

                        await _paymentRepository.UpdateAsync(payment.model);

                        return payment.model;
                    }
                    else if (mode == "subscription")
                    {
                        payment.model.Status = "Upfront-Paid";
                        payment.model.IsPaid = true;
                        payment.model.StripePaymentIntentId = stripeSession.InvoiceId;
                        payment.model.StripeSubscriptionId = stripeSession.SubscriptionId;

                        await _paymentRepository.UpdateAsync(payment.model);

                        return payment.model;
                    }
                    else 
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public Session GetStripeSessionBySessionId(string sessionId)
        {
            StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);

            return stripe.GetSessionStatus(sessionId);
        }

        public async Task<Session> GetStripeCheckoutForPayment(long userCourseId)
        {
            var userCourse = await _userCourseRepository.GetByIdAsync(userCourseId);
            if (userCourse != null)
            {
                StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);

                return stripe.CreatePaymentCheckoutSession(WebConfigurationManager.AppSettings["Domain"], userCourse.User.StripeCustomerId, userCourse.Cours.StripeProductId, (long)userCourse.Cours.Price);
            }
            else
            {
                return null;
            }
        }
        public async Task<Session> GetStripeCheckoutForSubscription(long userCourseId, int coursePlanId)
        {
            var userCourse = await _userCourseRepository.GetByIdAsync(userCourseId);
            if (userCourse != null && userCourse.Cours.CoursePlans.Any(i => i.Id == coursePlanId))
            {
                StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);

                return stripe.CreateCheckoutSession(WebConfigurationManager.AppSettings["Domain"], userCourse.User.StripeCustomerId, userCourse.Cours.StripeProductId, userCourse.Cours.CoursePlans?.Where(i => i.Id == coursePlanId).FirstOrDefault().StripePriceId, (long)userCourse.Cours.Price);
            }
            else
            {
                return null;
            }
        }
        public async Task<GenericResponseModel<PaymentVM>> CreateStripePayment(CardDetailsRequestModel cardDetailsRequestModel)
        {
            if (cardDetailsRequestModel.IsFullPayment)
            {
                return await CreateStripeFullPayment(cardDetailsRequestModel);
            }
            else if (!cardDetailsRequestModel.IsFullPayment && cardDetailsRequestModel.CoursePlanId != 0)
            {
                return await CreateStripeSubscriptionPayment(cardDetailsRequestModel);
            }
            else
            {
                return null;
            }
        }
        private async Task<GenericResponseModel<PaymentVM>> CreateStripeFullPayment(CardDetailsRequestModel cardDetailsRequestModel)
        {
            var userCourse = await _userCourseRepository.GetByIdAsync(cardDetailsRequestModel.UserCourseId);
            if (userCourse != null)
            {
                StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);

                //string stripeToken = stripe.CreateToken(cardDetailsRequestModel);

                //if (stripeToken != "0" && !stripeToken.Contains("stripe error: "))
                //{
                string stripeCharge = stripe.CreatePayment((long)userCourse.Cours.Price, "usd", cardDetailsRequestModel.StripeToken, "Cyfendry " + userCourse.Cours.Name + " Full Payment", userCourse.User.Email, userCourse.User.StripeCustomerId);

                if (stripeCharge != "0" && !stripeCharge.Contains("stripe error: "))
                {
                    PaymentVM paymentVM = new PaymentVM
                    {
                        CourseName = userCourse.Cours.Name,
                        Email = userCourse.User.Email,
                        StripePaymentId = stripeCharge
                    };

                    await _paymentRepository.CreateAsync(new Payment { IsPaid = true, PaidOn = DateTime.UtcNow, Status = "Full-Paid", StripePaymentId = stripeCharge, UserCourseId = cardDetailsRequestModel.UserCourseId });

                    return new GenericResponseModel<PaymentVM> { model = paymentVM, status = true, successMessage = "Payment completed!" };
                }
                else
                {
                    return new GenericResponseModel<PaymentVM> { errorMessage = stripeCharge, status = false, userMessage = stripeCharge };
                }
                //}
                //else
                //{
                //    return new GenericResponseModel<PaymentVM> { errorMessage = stripeToken, status = false, userMessage = stripeToken };
                //}
            }
            else
            {
                return new GenericResponseModel<PaymentVM> { errorMessage = "no user course found!", status = false, userMessage = "no user course found!" };
            }
        }
        private async Task<GenericResponseModel<PaymentVM>> CreateStripeSubscriptionPayment(CardDetailsRequestModel cardDetailsRequestModel)
        {
            var userCourse = await _userCourseRepository.GetByIdAsync(cardDetailsRequestModel.UserCourseId);
            if (userCourse != null && userCourse.Cours.CoursePlans.Any(i => i.Id == cardDetailsRequestModel.CoursePlanId))
            {
                StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);

                //string stripePaymentMethod = stripe.CreatePaymentMethod(userCourse.User.StripeCustomerId, cardDetailsRequestModel.Cvc, cardDetailsRequestModel.CardNumber, Convert.ToInt16(cardDetailsRequestModel.ExpMonth), Convert.ToInt16(cardDetailsRequestModel.ExpYear), cardDetailsRequestModel.StripeToken);

                //if (stripePaymentMethod != "0" && !stripePaymentMethod.Contains("stripe error: "))
                //{
                //stripe.AttachCustomerToPaymentMethod(userCourse.User.StripeCustomerId, stripePaymentMethod);
                //await _userPaymentMethodRepository.CreateAsync(new UserPaymentMethod { CardLastFour = cardDetailsRequestModel.CardNumber.Substring((cardDetailsRequestModel.CardNumber.Length - 4), 4), StripePaymentMethodId = stripePaymentMethod, UserId = userCourse.UserId });

                //string stripeToken = stripe.CreateToken(cardDetailsRequestModel);

                //if (stripeToken != "0" && !stripeToken.Contains("stripe error: "))
                //{
                //string stripeCharge = stripe.CreatePayment((long)userCourse.Cours.CoursePlans?.Where(i => i.Id == cardDetailsRequestModel.CoursePlanId).FirstOrDefault().UpFrontPrice, "usd", cardDetailsRequestModel.StripeToken, "Cyfendry " + userCourse.Cours.Name + " Full Payment", userCourse.User.Email, userCourse.User.StripeCustomerId);

                //if (stripeCharge != "0" && !stripeCharge.Contains("stripe error: "))
                //{

                string stripeSubscription = stripe.CreateSubscriptionSchedule(userCourse.User.StripeCustomerId, "price_1OBsWXF2CKNCk6PLijw9M37l", userCourse.Cours.CoursePlans?.Where(i => i.Id == cardDetailsRequestModel.CoursePlanId).FirstOrDefault().StripePriceId, "", userCourse.Cours.Name);

                string invoiceId = stripe.GetInvoice(userCourse.User.StripeCustomerId, stripeSubscription);

                stripe.FinalizeInvoice(invoiceId);

                await _subscriptionRepository.CreateAsync(new Subscription { StripeSubscriptionId = stripeSubscription, UserCourseId = cardDetailsRequestModel.UserCourseId });

                await _paymentRepository.CreateAsync(new Payment { IsPaid = true, PaidOn = DateTime.UtcNow, Status = "UpFront-Paid", StripePaymentId = invoiceId, UserCourseId = cardDetailsRequestModel.UserCourseId });

                PaymentVM paymentVM = new PaymentVM
                {
                    CourseName = userCourse.Cours.Name,
                    Email = userCourse.User.Email,
                    StripePaymentId = invoiceId,
                    StripeSubscriptionId = stripeSubscription
                };

                return new GenericResponseModel<PaymentVM> { model = paymentVM, status = true, successMessage = "Payment completed!" };
                //}
                //else
                //{
                //    return new GenericResponseModel<PaymentVM> { errorMessage = stripeCharge, status = false, userMessage = stripeCharge };
                //}
                //}
                //else
                //{
                //    return new GenericResponseModel<PaymentVM> { errorMessage = stripeToken, status = false, userMessage = stripeToken };
                //}
                //}
                //else
                //{
                //    return new GenericResponseModel<PaymentVM> { errorMessage = stripePaymentMethod, status = false, userMessage = stripePaymentMethod };
                //}
            }
            else
            {
                return new GenericResponseModel<PaymentVM> { errorMessage = "no user course found!", status = false, userMessage = "no user course found!" };
            }
        }
    }
}