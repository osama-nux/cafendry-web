using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.Models.ViewModels
{
    public class PaymentVM
    {
        public string Email { get; set; }
        public string StripePaymentId { get; set; }
        public string CourseName { get; set; }
        public string StripeSubscriptionId { get; set; }
    }
}