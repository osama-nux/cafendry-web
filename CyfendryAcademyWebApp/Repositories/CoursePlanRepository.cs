using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class CoursePlanRepository : ICoursePlanRepository
    {
        private admin_cyfendrydbEntities _dbContext;

        public CoursePlanRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<bool> CreateAsync(CoursePlan coursePlan)
        {
            _dbContext.CoursePlans.Add(coursePlan);

            return await CommitAsync();
        }
        private async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface ICoursePlanRepository
    {
        Task<bool> CreateAsync(CoursePlan coursePlan);
    }
}