using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class UserPaymentMethodRepository : IUserPaymentMethodRepository
    {
        private admin_cyfendrydbEntities _dbContext;

        public UserPaymentMethodRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<bool> CreateAsync(UserPaymentMethod userPaymentMethod)
        {
            userPaymentMethod.CreatedOn = DateTime.UtcNow;

            _dbContext.UserPaymentMethods.Add(userPaymentMethod);

            return await CommitAsync();
        }

        private async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface IUserPaymentMethodRepository
    {
        Task<bool> CreateAsync(UserPaymentMethod userPaymentMethod);
    }
}