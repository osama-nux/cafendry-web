using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.MapperProfile;
using CyfendryAcademyWebApp.Models;
using CyfendryAcademyWebApp.Models.RequestModels;
using CyfendryAcademyWebApp.Models.ViewModels;
using CyfendryAcademyWebApp.PaymentGateways;
using CyfendryAcademyWebApp.Repositories;
using System;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace CyfendryAcademyWebApp.Services
{
    public class AccountService
    {
        private IUserRepository _userRepository;
        private IUserCourseRepository _userCourseRepository;
        public AccountService()
        {
            admin_cyfendrydbEntities admin_CyfendrydbEntities = new admin_cyfendrydbEntities();

            _userRepository = new UserRepository(admin_CyfendrydbEntities);
            _userCourseRepository = new UserCourseRepository(admin_CyfendrydbEntities);
        }

        public async Task<GenericResponseModel<UserCourseVM>> RegisterAsync(RegisterRequestModel registerRequestModel)
        {
            var _user = await _userRepository.GetUserByEmailAsync(registerRequestModel.Email);
            if (_user == null)
            {
                var mapper = MapperConfig.Init();

                var user = mapper.Map<User>(registerRequestModel);
                user.UserRoleId = 1;
                user.StripeCustomerId = new StripeGateway(WebConfigurationManager.AppSettings["StripeApiKey"]).CreateCustomer(user.Email, user.FirstName, user.Phone);
                if (await _userRepository.CreateAsync(user))
                {
                    return await AddUserCourseAsync(user.Id, registerRequestModel.CourseId);
                }
                else
                {
                    return new GenericResponseModel<UserCourseVM> { errorMessage = "user creation failed!", status = false, userMessage = "something went wrong! please try again later." };
                }
            }
            else
            {
                var userCourse = await _userCourseRepository.IsExistAsync(_user.Id, registerRequestModel.CourseId);

                if (userCourse == null)
                {
                    return await AddUserCourseAsync(_user.Id, registerRequestModel.CourseId);
                }

                return new GenericResponseModel<UserCourseVM> { errorMessage = "user already exist!", status = false, userMessage = "you have already an account!" };
            }
        }
        
        private async Task<GenericResponseModel<UserCourseVM>> AddUserCourseAsync(long userId, long courseId)
        {
            try
            {
                UserCours userCours = new UserCours { CourseId = courseId, UserId = userId };

                if (await _userCourseRepository.CreateAsync(userCours))
                {
                    var mapper = MapperConfig.Init();

                    var _userCourse = mapper.Map<UserCourseVM>(userCours);
                    return new GenericResponseModel<UserCourseVM> { model = _userCourse, status = true, successMessage = "user registered!" };
                }
                else
                {
                    return new GenericResponseModel<UserCourseVM> { errorMessage = "user course creation failed!", status = false, userMessage = "something went wrong! please try again later." };
                }
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
    }
}