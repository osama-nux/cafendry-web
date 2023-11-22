using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CyfendryAcademyWebApp.Models.RequestModels
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "First Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "first name must contain only alphabetic characters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "last name must contain only alphabetic characters")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "The Email Address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "The Password is required")]
        [MaxLength(15, ErrorMessage = "Password max length is 15")]
        [MinLength(8, ErrorMessage = "Password min length is 8")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please select course")]
        public long CourseId { get; set; }
    }
}