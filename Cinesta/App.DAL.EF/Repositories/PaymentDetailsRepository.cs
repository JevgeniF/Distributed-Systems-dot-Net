using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class PaymentDetailsRepository : BaseEntityRepository<DTO.PaymentDetails, PaymentDetails, AppDbContext>,
    IPaymentDetailsRepository
{
    public PaymentDetailsRepository(AppDbContext dbContext, IMapper<DTO.PaymentDetails, PaymentDetails> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DTO.PaymentDetails>> IncludeGetAllByUserIdAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(p => p.AppUser).Where(p => p.AppUserId == userId);

        return (await query.ToListAsync()).Select(p => Mapper.Map(p)!);
    }
}