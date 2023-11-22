using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class UserCourseRepository : IUserCourseRepository
    {
        private admin_cyfendrydbEntities _dbContext;
        public UserCourseRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<UserCours> GetByCustomerIdAndProductId(string customerId, string productId)
        {
            var user = await _dbContext.Users.Where(i => i.StripeCustomerId == customerId).FirstOrDefaultAsync();

            var course = await _dbContext.Courses.Where(i => i.StripeProductId == productId).FirstOrDefaultAsync();

            return await _dbContext.UserCourses.Where(i => i.Id == user.Id && i.CourseId == course.Id).FirstOrDefaultAsync();
        }
        public async Task<UserCours> GetByIdAsync(long id)
        {
            return await _dbContext.UserCourses.FindAsync(id);
        }
        public async Task<UserCours> IsExistAsync(long userId, long courseId)
        {
            return await _dbContext.UserCourses.Where(i => i.UserId == userId && i.CourseId == courseId).FirstOrDefaultAsync();
        }
        public async Task<bool> CreateAsync(UserCours userCours)
        {
            userCours.CreatedOn = DateTime.UtcNow;

            _dbContext.UserCourses.Add(userCours);

            return await CommitAsync();
        }

        private async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface IUserCourseRepository
    {
        Task<UserCours> GetByIdAsync(long id);
        Task<UserCours> IsExistAsync(long userId, long courseId);
        Task<bool> CreateAsync(UserCours userCours);
        Task<UserCours> GetByCustomerIdAndProductId(string customerId, string productId);
    }
}