using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class UserRoleRepository
    {
        private admin_cyfendrydbEntities _dbContext = new admin_cyfendrydbEntities();

        public async Task<List<UserRole>> GetAllAsync()
        {
            return await _dbContext.UserRoles.Where(i => i.IsDeleted == false).ToListAsync();
        }
    }
}