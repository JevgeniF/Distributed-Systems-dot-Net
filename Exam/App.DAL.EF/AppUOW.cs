using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{
    private readonly IMapper _mapper;

    private ISubscriptionRepository? _subscription;

    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public virtual ISubscriptionRepository Subscription =>
        _subscription ??= new SubscriptionRepository(UOWDbContext, new SubscriptionMapper(_mapper));
}