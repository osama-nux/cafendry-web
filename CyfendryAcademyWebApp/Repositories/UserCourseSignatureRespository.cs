using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class UserCourseSignatureRespository : IUserCourseSignatureRespository
    {
        private admin_cyfendrydbEntities _dbContext;
        public UserCourseSignatureRespository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<bool> CreateAsync(UserCourseSignature userCourseSignature)
        {
            userCourseSignature.IsDeleted = false;
            userCourseSignature.CreatedOn = DateTime.UtcNow;

            _dbContext.UserCourseSignatures.Add(userCourseSignature);

            return await CommitAsync();
        }
        private async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface IUserCourseSignatureRespository
    {
        Task<bool> CreateAsync(UserCourseSignature userCourseSignature);
    }
}