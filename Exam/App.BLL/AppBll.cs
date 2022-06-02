using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBll : BaseBll<IAppUOW>, IAppBll
{
    private readonly IMapper _mapper;

    private ISubscriptionService? _subscription;
    protected IAppUOW UOW;

    public AppBll(IAppUOW uow, IMapper mapper)
    {
        UOW = uow;
        _mapper = mapper;
    }

    public override async Task<int> SaveChangesAsync()
    {
        return await UOW.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return UOW.SaveChanges();
    }

    public virtual ISubscriptionService Subscription =>
        _subscription ??= new SubscriptionService(UOW.Subscription, new SubscriptionMapper(_mapper));
}