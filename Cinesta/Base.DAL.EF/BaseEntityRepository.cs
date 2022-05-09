using Base.Contracts;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TAppEntity, TDalEntity, TDbContext> : BaseEntityRepository<TAppEntity, TDalEntity, Guid, TDbContext>
    where TAppEntity : class, IDomainEntityId<Guid>
    where TDalEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IMapper<TAppEntity, TDalEntity> mapper) : base(dbContext, mapper)
    {
    }
}

public class BaseEntityRepository<TAppEntity,TDalEntity, TKey, TDbContext> : IEntityRepository<TAppEntity, TKey>
    where TAppEntity : class, IDomainEntityId<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDalEntity> RepoDbSet;
    protected readonly IMapper<TAppEntity, TDalEntity> Mapper;

    public BaseEntityRepository(TDbContext dbContext, IMapper<TAppEntity, TDalEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = dbContext.Set<TDalEntity>();
        Mapper = mapper;
    }

    public virtual TAppEntity Add(TAppEntity entity)
    {
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TAppEntity Update(TAppEntity entity)
    {
        return Mapper.Map(RepoDbSet.Update(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TAppEntity Remove(TAppEntity entity)
    {
        return Mapper.Map(RepoDbSet.Remove(Mapper.Map(entity)!).Entity)!;
}

    public virtual TAppEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TAppEntity).Name} was not found");
        return Remove(entity);
    }

    public virtual TAppEntity? FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(noTracking).FirstOrDefault(e => e.Id.Equals(id)));
    }

    public virtual IEnumerable<TAppEntity> GetAll(bool noTracking = true)
    {
        return CreateQuery(noTracking).ToList().Select(e => Mapper.Map(e)!);
    }

    public virtual bool Exists(TKey id)
    {
        return RepoDbSet.Any(e => e.Id.Equals(id));
    }

    public virtual async Task<TAppEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await CreateQuery(noTracking).FirstOrDefaultAsync(e => e.Id.Equals(id)));
    }

    public virtual async Task<IEnumerable<TAppEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await CreateQuery(noTracking).ToListAsync()).Select(e => Mapper.Map(e)!);
    }

    public virtual async Task<bool> ExistsAsync(TKey id)
    {
        return await RepoDbSet.AnyAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<TAppEntity> RemoveAsync(TKey id)
    {
        var entity = await FirstOrDefaultAsync(id);
        if (entity == null)
            // TODO: implement custom exception for entity not found
            throw new NullReferenceException($"Entity {typeof(TAppEntity).Name} was not found");
        return Remove(entity);
    }

    protected virtual IQueryable<TDalEntity> CreateQuery(bool noTracking = true)
    {
        // TODO: entity ownership control

        var query = RepoDbSet.AsQueryable();
        if (noTracking) query = query.AsNoTracking();

        return query;
    }
}