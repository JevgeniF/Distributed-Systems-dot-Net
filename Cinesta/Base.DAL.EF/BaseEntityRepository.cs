using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Base.Contracts.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class
    BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext> : BaseEntityRepository<TDalEntity, TDomainEntity, Guid,
        TDbContext>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDomainEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper) : base(dbContext,
        mapper)
    {
    }
}

public class BaseEntityRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IEntityRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly IMapper<TDalEntity, TDomainEntity> Mapper;
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;

    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} was not found");
        return Remove(entity);
    }

    public virtual TDalEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(e => e.Id.Equals(id)));
    }

    public virtual IEnumerable<TDalEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e)!);
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(e => e.Id.Equals(id));
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id)));
    }

    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e)!);
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<TDalEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} was not found");
        return Remove(entity);
    }

    protected virtual IQueryable<TDomainEntity> CreateQuery(bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        return query;
    }
}