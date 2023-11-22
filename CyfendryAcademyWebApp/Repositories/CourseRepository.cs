using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private admin_cyfendrydbEntities _dbContext;

        public CourseRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<bool> UpdateAsync(Cours cours)
        {
            _dbContext.Entry(cours).State = EntityState.Modified;

            return await CommitAsync();
        }
        public async Task<List<Cours>> GetAllActiveAsync()
        {
            return await _dbContext.Courses.Where(i => i.IsActive == true && i.IsDeleted == false).ToListAsync();
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
    public interface ICourseRepository
    {
        Task<bool> UpdateAsync(Cours cours);
        Task<List<Cours>> GetAllActiveAsync();
    }
}