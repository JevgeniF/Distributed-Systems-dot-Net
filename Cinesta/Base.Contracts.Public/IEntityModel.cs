using Base.Contracts.BLL;
using Base.Contracts.Domain;

namespace Base.Contracts.Public;

public interface IEntityModel<TEntity> : IEntityService<TEntity>, IEntityModel<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}

public interface IEntityModel<TEntity, TKey> : IEntityService<TEntity, TKey>
    where TEntity : class, IDomainEntityId<TKey> where TKey : IEquatable<TKey>
{
}