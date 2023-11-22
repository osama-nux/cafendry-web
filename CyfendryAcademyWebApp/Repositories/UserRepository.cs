using CyfendryAcademyWebApp.Data;
using CyfendryAcademyWebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace CyfendryAcademyWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private admin_cyfendrydbEntities _dbContext;
        public UserRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users.Where(i => i.Email == email).FirstOrDefaultAsync();
        }
        public async Task<bool> CreateAsync(User user)
        {
            user.CreatedOn = DateTime.UtcNow;
            user.IsActive = true;
            user.IsDeleted = false;

            user.Salt = (Int64)(user.CreatedOn.Value - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            string HashPass = new CryptoHelper().RetrieveHash(user.Password, Convert.ToString(user.Salt));
            user.Password = HashPass;

            _dbContext.Users.Add(user);

            return await CommitAsync();
        }
        public async Task<bool> UpdateAsync(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;

            return await CommitAsync();
        }

        private async Task<bool> CommitAsync()
        {
            try
            {
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
    }
}