using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class SubscriptionRepository : BaseEntityRepository<DTO.Subscription, Subscription, AppDbContext>,
    ISubscriptionRepository
{
    public SubscriptionRepository(AppDbContext dbContext, IMapper<DTO.Subscription, Subscription> mapper) : base(
        dbContext, mapper)
    {
    }
}