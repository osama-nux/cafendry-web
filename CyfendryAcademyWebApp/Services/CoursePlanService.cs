using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.PaymentGateways;
using CyfendryAcademyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace CyfendryAcademyWebApp.Services
{
    public class CoursePlanService
    {
        private ICourseRepository _courseRepository;
        private ICoursePlanRepository _coursePlanRepository;

        public CoursePlanService()
        {
            admin_cyfendrydbEntities admin_CyfendrydbEntities = new admin_cyfendrydbEntities();

            _courseRepository = new CourseRepository(admin_CyfendrydbEntities);
            _coursePlanRepository = new CoursePlanRepository(admin_CyfendrydbEntities);
        }

        public async Task<bool> AddStripePriceId()
        {
            StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);
            var courses = await _courseRepository.GetAllActiveAsync();

            foreach (var course in courses)
            {
                if (course.CoursePlans == null || course.CoursePlans.Count == 0)
                {
                    if (course.Price == 3300)
                    {
                        decimal totalPrice = (decimal)course.Price;

                        //1
                        CoursePlan coursePlan1 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 2,
                            UpFrontPrice = 200
                        };

                        coursePlan1.PlanPrice = (totalPrice - coursePlan1.UpFrontPrice) / coursePlan1.IntervalValue;

                        string stripePriceId1 = stripe.CreatePrice((long)coursePlan1.PlanPrice, course.StripeProductId, (int)coursePlan1.IntervalValue);

                        coursePlan1.StripePriceId = stripePriceId1;

                        await _coursePlanRepository.CreateAsync(coursePlan1);

                        //2
                        CoursePlan coursePlan2 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 3,
                            UpFrontPrice = 1320
                        };

                        coursePlan2.PlanPrice = (totalPrice - coursePlan2.UpFrontPrice) / coursePlan2.IntervalValue;

                        string stripePriceId2 = stripe.CreatePrice((long)coursePlan2.PlanPrice, course.StripeProductId, (int)coursePlan2.IntervalValue);

                        coursePlan2.StripePriceId = stripePriceId2;

                        await _coursePlanRepository.CreateAsync(coursePlan2);

                        //3
                        CoursePlan coursePlan3 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 4,
                            UpFrontPrice = 660
                        };

                        coursePlan3.PlanPrice = (totalPrice - coursePlan3.UpFrontPrice) / coursePlan3.IntervalValue;

                        string stripePriceId3 = stripe.CreatePrice((long)coursePlan3.PlanPrice, course.StripeProductId, (int)coursePlan3.IntervalValue);

                        coursePlan3.StripePriceId = stripePriceId3;

                        await _coursePlanRepository.CreateAsync(coursePlan3);
                    }
                    else if (course.Price == 3000)
                    {
                        decimal totalPrice = (decimal)course.Price;

                        //1
                        CoursePlan coursePlan1 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 2,
                            UpFrontPrice = 1500
                        };

                        coursePlan1.PlanPrice = (totalPrice - coursePlan1.UpFrontPrice) / coursePlan1.IntervalValue;

                        string stripePriceId1 = stripe.CreatePrice((long)coursePlan1.PlanPrice, course.StripeProductId, (int)coursePlan1.IntervalValue);

                        coursePlan1.StripePriceId = stripePriceId1;

                        await _coursePlanRepository.CreateAsync(coursePlan1);

                        //2
                        CoursePlan coursePlan2 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 3,
                            UpFrontPrice = 1200
                        };

                        coursePlan2.PlanPrice = (totalPrice - coursePlan2.UpFrontPrice) / coursePlan2.IntervalValue;

                        string stripePriceId2 = stripe.CreatePrice((long)coursePlan2.PlanPrice, course.StripeProductId, (int)coursePlan2.IntervalValue);

                        coursePlan2.StripePriceId = stripePriceId2;

                        await _coursePlanRepository.CreateAsync(coursePlan2);

                        //3
                        CoursePlan coursePlan3 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 4,
                            UpFrontPrice = 600
                        };

                        coursePlan3.PlanPrice = (totalPrice - coursePlan3.UpFrontPrice) / coursePlan3.IntervalValue;

                        string stripePriceId3 = stripe.CreatePrice((long)coursePlan3.PlanPrice, course.StripeProductId, (int)coursePlan3.IntervalValue);

                        coursePlan3.StripePriceId = stripePriceId3;

                        await _coursePlanRepository.CreateAsync(coursePlan3);
                    }
                    else if (course.Price == 4500)
                    {
                        decimal totalPrice = (decimal)course.Price;

                        //1
                        CoursePlan coursePlan1 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 2,
                            UpFrontPrice = 1700
                        };

                        coursePlan1.PlanPrice = (totalPrice - coursePlan1.UpFrontPrice) / coursePlan1.IntervalValue;

                        string stripePriceId1 = stripe.CreatePrice((long)coursePlan1.PlanPrice, course.StripeProductId, (int)coursePlan1.IntervalValue);

                        coursePlan1.StripePriceId = stripePriceId1;

                        await _coursePlanRepository.CreateAsync(coursePlan1);

                        //2
                        CoursePlan coursePlan2 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 3,
                            UpFrontPrice = 1500
                        };

                        coursePlan2.PlanPrice = (totalPrice - coursePlan2.UpFrontPrice) / coursePlan2.IntervalValue;

                        string stripePriceId2 = stripe.CreatePrice((long)coursePlan2.PlanPrice, course.StripeProductId, (int)coursePlan2.IntervalValue);

                        coursePlan2.StripePriceId = stripePriceId2;

                        await _coursePlanRepository.CreateAsync(coursePlan2);

                        //3
                        CoursePlan coursePlan3 = new CoursePlan
                        {
                            CourseId = course.Id,
                            Interval = "week",
                            IntervalValue = 4,
                            UpFrontPrice = 900
                        };

                        coursePlan3.PlanPrice = (totalPrice - coursePlan3.UpFrontPrice) / coursePlan3.IntervalValue;

                        string stripePriceId3 = stripe.CreatePrice((long)coursePlan3.PlanPrice, course.StripeProductId, (int)coursePlan3.IntervalValue);

                        coursePlan3.StripePriceId = stripePriceId3;

                        await _coursePlanRepository.CreateAsync(coursePlan3);
                    }
                }
            }
            return false;
        }
    }
}