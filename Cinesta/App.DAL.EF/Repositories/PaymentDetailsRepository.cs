using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PaymentDetailsRepository : BaseEntityRepository<PaymentDetails, Domain.PaymentDetails, AppDbContext>,
    IPaymentDetailsRepository
{
    public PaymentDetailsRepository(AppDbContext dbContext, IMapper<PaymentDetails, Domain.PaymentDetails> mapper) :
        base(
            dbContext, mapper)
    {
    }

    public async Task<IEnumerable<PaymentDetails>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.AppUser).Where(p => p.AppUserId == userId);

        return (await query.ToListAsync()).Select(p => Mapper.Map(p)!);
    }
}