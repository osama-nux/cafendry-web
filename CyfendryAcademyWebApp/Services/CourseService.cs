using AutoMapper;
using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.MapperProfile;
using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Models.RequestModels;
using CyfendryAcademyWebApp.Models.ViewModels;
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
    public class CourseService
    {
        private ICourseRepository _courseRepository;
        public CourseService()
        {
            admin_cyfendrydbEntities admin_CyfendrydbEntities = new admin_cyfendrydbEntities();

            _courseRepository = new CourseRepository(admin_CyfendrydbEntities);
        }

        public async Task<GenericResponseModel<CourseListVM>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllActiveAsync();

            if (courses != null && courses.Count > 0)
            {
                var mapper = MapperConfig.Init();
                var courseListVM = mapper.Map<List<CourseListVM>>(courses);

                return new GenericResponseModel<CourseListVM> { modelList = courseListVM, status = true, successMessage = "successfully found courses." };
            }
            else
            {
                return new GenericResponseModel<CourseListVM> { errorMessage = "no courses found!", status = false, userMessage = "no courses added yet!"};
            }
        }
        public async Task<bool> AddStripeProductId()
        {
            StripeGateway stripe = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]);
            var courses = await _courseRepository.GetAllActiveAsync();

            foreach (var course in courses)
            {
                if (course.StripeProductId == null)
                {
                    string stripeProductId = stripe.CreateProduct(course.Name);

                    course.StripeProductId = stripeProductId;

                    await _courseRepository.UpdateAsync(course);
                }
            }

            return true;
        }
    }
}