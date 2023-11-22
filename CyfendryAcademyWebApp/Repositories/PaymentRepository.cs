using CyfendryAcademyWebApp.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CyfendryAcademyWebApp.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private admin_cyfendrydbEntities _dbContext;
        public PaymentRepository(admin_cyfendrydbEntities admin_CyfendrydbEntities)
        {
            _dbContext = admin_CyfendrydbEntities;
        }

        public async Task<Payment> GetBySessionIdAsync(string sessionId)
        {
            return await _dbContext.Payments.FirstOrDefaultAsync(i => i.StripeSessionId == sessionId);
        }
        public async Task<bool> CreateAsync(Payment payment)
        {
            payment.CreatedOn = DateTime.UtcNow;
            payment.IsDeleted = false;

            _dbContext.Payments.Add(payment);

            return await CommitAsync();
        }
        public async Task<bool> UpdateAsync(Payment payment)
        {
            _dbContext.Entry(payment).State = EntityState.Modified;

            return await CommitAsync();
        }

        private async Task<bool> CommitAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
    public interface IPaymentRepository
    {
        Task<Payment> GetBySessionIdAsync(string sessionId);
        Task<bool> CreateAsync(Payment payment);
        Task<bool> UpdateAsync(Payment payment);
    }
}