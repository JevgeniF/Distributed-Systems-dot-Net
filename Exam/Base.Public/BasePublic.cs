using Base.Contracts.BLL;
using Base.Contracts.Public;

namespace Base.Public;

public abstract class BasePublic<TBll> : IPublic
    where TBll : IBll
{
    public abstract Task<int> SaveChangesAsync();
    public abstract int SaveChanges();
}