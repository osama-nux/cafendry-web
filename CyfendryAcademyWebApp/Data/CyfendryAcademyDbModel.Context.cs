﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class admin_cyfendrydbEntities : DbContext
    {
        public admin_cyfendrydbEntities()
            : base("name=admin_cyfendrydbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cours> Courses { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<UserCours> UserCourses { get; set; }
        public virtual DbSet<UserPaymentMethod> UserPaymentMethods { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CoursePlan> CoursePlans { get; set; }
        public virtual DbSet<UserCourseSignature> UserCourseSignatures { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
    }
}
