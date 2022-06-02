using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class SubscriptionRepository : BaseEntityRepository<Subscription, Domain.Subscription, AppDbContext>,
    ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext dbContext, IMapper<Subscription, Domain.Subscription> mapper) : base(
        dbContext, mapper)
    {
    }
}