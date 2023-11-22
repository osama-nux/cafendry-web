using CyfendryAcademyWebApp.Models.RequestModels;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;

namespace CyfendryAcademyWebApp.PaymentGateways
{
    public class StripeGateway
    {
        private string ErrorMessage = "stripe error: ";
        public StripeGateway(string secretKey)
        {
            StripeConfiguration.ApiKey = secretKey;
        }

        public string CreateToken(CardDetailsRequestModel cardDetailsDto)
        {
            try
            {
                var options = new TokenCreateOptions
                {
                    Card = new TokenCardOptions
                    {
                        Number = cardDetailsDto.CardNumber,
                        ExpMonth = cardDetailsDto.ExpMonth,
                        ExpYear = cardDetailsDto.ExpYear,
                        Cvc = cardDetailsDto.Cvc,
                    },
                };
                var service = new TokenService();
                var token = service.Create(options);
                return token != null ? token.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string CreatePayment(long amount, string currency, string token, string description, string email, string customerId)
        {
            try
            {
                var options = new ChargeCreateOptions
                {
                    Amount = amount * 100,
                    Currency = currency,
                    Source = token,
                    Description = description,
                    ReceiptEmail = email,
                    //Customer = customerId,
                    Capture = true
                };
                var service = new ChargeService();
                var charge = service.Create(options);
                return charge != null ? charge.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string CreateCustomer(string email, string fullName, string phone)
        {
            try
            {
                var options = new CustomerCreateOptions
                {
                    Description = "Cyfendry Student",
                    Email = email,
                    Name = fullName,
                    Phone = phone
                };
                var service = new CustomerService();
                var customer = service.Create(options);
                return customer != null ? customer.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string CreatePaymentMethod(string customerStripeId, string cvc, string cardNumber, long expMonth, long expYear, string token = null)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var option = new PaymentMethodCreateOptions
                {
                    //Customer = customerStripeId,
                    //Card = new PaymentMethodCardOptions { Cvc = cvc, ExpMonth = expMonth, ExpYear = expYear, Number = cardNumber },
                    //Type = "card",
                    PaymentMethod = token
                };
                var service = new PaymentMethodService();
                var paymentMethod = service.Create(option);
                return paymentMethod != null ? paymentMethod.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public void AttachCustomerToPaymentMethod(string customerStripeId, string paymentMethodId)
        {
            var options = new PaymentMethodAttachOptions
            {
                Customer = customerStripeId
            };
            var service = new PaymentMethodService();
            service.Attach(
              paymentMethodId,
              options);
        }
        public string CreatePrice(long amount, string productId, int interval)
        {
            try
            {
                var options = new PriceCreateOptions
                {
                    UnitAmount = amount,
                    Currency = "usd",
                    Recurring = new PriceRecurringOptions
                    {
                        Interval = "week",
                        IntervalCount = interval
                    },
                    Product = productId,
                };
                var service = new PriceService();
                var price = service.Create(options);
                return price != null ? price.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string CreateProduct(string productName)
        {
            try
            {
                var options = new ProductCreateOptions
                {
                    Name = productName
                };
                var service = new ProductService();
                var product = service.Create(options);
                return product != null ? product.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string CreateSubscription(string customerId, string priceId, string paymentMethodId, string courseName)
        {
            try
            {
                var options = new SubscriptionCreateOptions
                {
                    Customer = customerId,
                    Items = new List<SubscriptionItemOptions>
                    {
                      new SubscriptionItemOptions
                      {
                        Price = priceId,
                        Quantity = 1,

                      }
                    },
                    //DefaultPaymentMethod = paymentMethodId,
                    Currency = "usd",
                    Description = courseName + " Subscription",
                    AddInvoiceItems = new List<SubscriptionAddInvoiceItemOptions>
                    {
                        new SubscriptionAddInvoiceItemOptions
                        {

                        }
                    }
                };
                var service = new SubscriptionService();
                var subscription = service.Create(options);
                return subscription != null ? subscription.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public Session GetSessionStatus(string sessionId)
        {
            var service = new SessionService();

            return service.Get(sessionId);
        }
        public Session CreatePaymentCheckoutSession(string domain, string customerId, string productId, long price)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + "Signature/Upload?customerId=" + customerId + "&productId=" + productId,
                    CancelUrl = domain + "Payment/Failed",
                    Customer = customerId,
                    LineItems = new List<SessionLineItemOptions>
                    {
                      new SessionLineItemOptions
                      {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Product = productId,
                            Currency = "usd",
                            UnitAmount = price * 100
                        },
                        Quantity = 1,
                      }
                    },
                    Mode = "payment",
                };
                var service = new SessionService();
                var session = service.Create(options);

                return session;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public Session CreateCheckoutSession(string domain, string customerId, string productId, string priceId, long upFrontPrice)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + "Signature/Upload?customerId=" + customerId + "&productId=" + productId,
                    CancelUrl = domain + "Payment/Failed",
                    Customer = customerId,
                    LineItems = new List<SessionLineItemOptions>
                    {
                          new SessionLineItemOptions
                          {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Product = productId,
                                Currency = "usd",
                                UnitAmount = upFrontPrice * 100
                            },
                            Quantity = 1,
                          },
                          new SessionLineItemOptions
                          {
                            Price = priceId,
                            Quantity = 1,
                          }
                    },
                    Mode = "subscription",
                };
                var service = new SessionService();
                var session = service.Create(options);

                return session;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string CreateSubscriptionSchedule(string customerId, string upfrontPriceId, string priceId, string paymentMethodId, string courseName)
        {
            try
            {
                var options = new SubscriptionScheduleCreateOptions
                {
                    Customer = customerId,
                    StartDate = DateTime.Now,
                    Phases = new List<SubscriptionSchedulePhaseOptions>
                    {
                        new SubscriptionSchedulePhaseOptions
                        {
                            Items = new List<SubscriptionSchedulePhaseItemOptions>
                            {
                                new SubscriptionSchedulePhaseItemOptions
                                {
                                    Price = upfrontPriceId,
                                    Quantity = 1
                                }
                            },
                            Iterations = 1
                        },
                        new SubscriptionSchedulePhaseOptions
                        {
                            Items = new List<SubscriptionSchedulePhaseItemOptions>
                            {
                                new SubscriptionSchedulePhaseItemOptions
                                {
                                    Price = priceId,
                                    Quantity = 1
                                }
                            },
                            Iterations = 1
                        }
                    }
                };
                var service = new SubscriptionScheduleService();
                var subscription = service.Create(options);
                return subscription != null ? subscription.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string GetInvoice(string customerId, string subscriptionId)
        {
            var options = new InvoiceListOptions
            {
                Limit = 3,
                Status = "draft",
                Customer = customerId,
                Subscription = subscriptionId
            };
            var service = new InvoiceService();
            StripeList<Invoice> invoices = service.List(
              options);

            Invoice invoice = invoices.Data[0];
            return invoice.Id;
        }
        public void FinalizeInvoice(string invoiceId)
        {
            var service = new InvoiceService();
            service.FinalizeInvoice(
              invoiceId);
        }
        public string CreateAccount()
        {
            try
            {
                var options = new AccountCreateOptions { Type = "express" };
                var service = new AccountService();
                var account = service.Create(options);
                return account != null ? account.Id : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
        public string CreateAccountLink(string accountId)
        {
            try
            {
                var options = new AccountLinkCreateOptions
                {
                    Account = accountId,
                    RefreshUrl = "https://example.com/reauth",
                    ReturnUrl = "https://example.com/return",
                    Type = "account_onboarding",
                };
                var service = new AccountLinkService();
                var accountLink = service.Create(options);

                return accountLink != null ? accountLink.Url : "0";
            }
            catch (System.Exception ex)
            {
                return ErrorMessage + ex.Message;
            }
        }
    }
}