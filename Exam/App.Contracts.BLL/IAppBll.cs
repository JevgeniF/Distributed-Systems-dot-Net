using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBll : IBll
{
    ISubscriptionService Subscription { get; }
}