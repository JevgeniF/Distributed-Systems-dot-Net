using App.Contracts.BLL;
using App.Contracts.Public;
using App.Contracts.Public.Models;
using App.Public.Mappers;
using App.Public.Models;
using AutoMapper;
using Base.Public;

namespace App.Public;

public class AppPublic : BasePublic<IAppBll>, IAppPublic
{
    private readonly IMapper _mapper;

    private ISubscriptionModel? _subscription;
    protected IAppBll Bll;

    public AppPublic(IAppBll bll, IMapper mapper)
    {
        Bll = bll;
        _mapper = mapper;
    }

    public override async Task<int> SaveChangesAsync()
    {
        return await Bll.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return Bll.SaveChanges();
    }

    public virtual ISubscriptionModel Subscription =>
        _subscription ??= new SubscriptionModel(Bll.Subscription, new SubscriptionMapper(_mapper));
}