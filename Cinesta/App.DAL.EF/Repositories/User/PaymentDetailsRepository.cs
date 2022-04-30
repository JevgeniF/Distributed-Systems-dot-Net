using App.Contracts.DAL.User;
using App.Domain.User;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories.User;

public class PaymentDetailsRepository : BaseEntityRepository<PaymentDetails, AppDbContext>, IPaymentDetailsRepository
{
    public PaymentDetailsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<PaymentDetails>> GetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.AppUser).Where(p => p.AppUserId == userId);
        
        return await query.ToListAsync();
    }
}