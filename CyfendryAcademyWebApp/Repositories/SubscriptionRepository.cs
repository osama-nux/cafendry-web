using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private admin_cyfendrydbEntities _dbContext;

        public SubscriptionRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<bool> CreateAsync(Subscription subscription)
        {
            subscription.CreatedOn = DateTime.UtcNow;

            _dbContext.Subscriptions.Add(subscription);

            return await CommitAsync();
        }

        private async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface ISubscriptionRepository
    {
        Task<bool> CreateAsync(Subscription subscription);
    }
}