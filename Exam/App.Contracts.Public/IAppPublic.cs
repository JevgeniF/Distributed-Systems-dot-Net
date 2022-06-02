using App.Contracts.Public.Models;
using Base.Contracts.Public;

namespace App.Contracts.Public;

public interface IAppPublic : IPublic
{
    ISubscriptionModel Subscription { get; }
}