//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CyfendryAcademyWebApp.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserPaymentMethod
    {
        public long Id { get; set; }
        public Nullable<long> UserId { get; set; }
        public string StripePaymentMethodId { get; set; }
        public string CardLastFour { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual User User { get; set; }
    }
}
