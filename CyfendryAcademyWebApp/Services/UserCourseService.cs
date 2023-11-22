using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Services
{
    public class UserCourseService
    {
        private IUserCourseRepository _userCourseRepository;

        public UserCourseService()
        {
            admin_cyfendrydbEntities admin_CyfendrydbEntities = new admin_cyfendrydbEntities();

            _userCourseRepository = new UserCourseRepository(admin_CyfendrydbEntities);
        }

        public async Task<GenericResponseModel<UserCours>> GetByCustomerIdAndProductIdAsync(string customerId, string productId)
        {
            var userCourse = await _userCourseRepository.GetByCustomerIdAndProductId(customerId, productId);

            if (userCourse != null)
            {
                return new GenericResponseModel<UserCours> { model = userCourse, status = true, successMessage = "user course found." };
            }
            else
            {
                return new GenericResponseModel<UserCours> { errorMessage = "user course not found!", status = false, userMessage = "something went wrong! please try again later." };
            }
        }
        public async Task<GenericResponseModel<UserCours>> AddUserCourseAsync(long userId, long courseId)
        {
            UserCours userCours = new UserCours { CourseId = courseId, UserId = userId };

            if (await _userCourseRepository.CreateAsync(userCours))
            {
                return new GenericResponseModel<UserCours> { model = userCours, status = true, successMessage = "course successfully added." };
            }
            else
            {
                return new GenericResponseModel<UserCours> { errorMessage = "user course creation failed!", status = false, userMessage = "something went wrong! please try again later." };
            }
        }
    }
}