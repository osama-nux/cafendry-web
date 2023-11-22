using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.Models.RequestModels
{
    public class CardDetailsRequestModel
    {
        [Required]
        public long UserCourseId { get; set; }
        [Required]
        public long CoursePlanId { get; set; }
        [Required(ErrorMessage = "Card number is required")]
        public string CardNumber { get; set; }
        [Required(ErrorMessage = "Card expiry month is required")]
        [MaxLength(2, ErrorMessage = "Expiry month should be of 2 digits")]
        [MinLength(2, ErrorMessage = "Expiry month should be of 2 digits")]
        public string ExpMonth { get; set; }
        [Required(ErrorMessage = "Card expiry year is required")]
        [MaxLength(4, ErrorMessage = "Expiry year should be of 4 digits")]
        [MinLength(4, ErrorMessage = "Expiry year should be of 4 digits")]
        public string ExpYear { get; set; }
        [Required(ErrorMessage = "Card cvc/cvv is required")]
        [MaxLength(3, ErrorMessage = "CVC/CVV should be of 3 digits")]
        [MinLength(3, ErrorMessage = "CVC/CVV should be of 3 digits")]
        public string Cvc { get; set; }
        [Required]
        public bool IsFullPayment { get; set; }
        public string StripeToken { get; set; }
    }
}