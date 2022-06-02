using Base.Contracts.BLL;
using Base.Contracts.Domain;
using Base.Contracts.Mapper;
using Base.Contracts.Public;

namespace Base.Public;

public class BaseEntityModel<TPublicEntity, TBllEntity, TService> :
    BaseEntityModel<TPublicEntity, TBllEntity, TService, Guid>, IEntityModel<TPublicEntity>
    where TPublicEntity : class, IDomainEntityId
    where TBllEntity : class, IDomainEntityId
    where TService : IEntityService<TBllEntity>
{
    public BaseEntityModel(TService service, IMapper<TPublicEntity, TBllEntity> mapper) : base(service, mapper)
    {
    }
}

public class BaseEntityModel<TPublicEntity, TBllEntity, TService, TKey> : IEntityModel<TPublicEntity, TKey>
    where TPublicEntity : class, IDomainEntityId<TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TService : IEntityService<TBllEntity, TKey>
    where TKey : IEquatable<TKey>
{
    protected IMapper<TPublicEntity, TBllEntity> Mapper;

    public BaseEntityModel(TService service, IMapper<TPublicEntity, TBllEntity> mapper)
    {
        Service = service;
        Mapper = mapper;
    }

    protected TService Service { get; set; }

    public TPublicEntity Add(TPublicEntity entity)
    {
        return Mapper.Map(Service.Add(Mapper.Map(entity)!))!;
    }

    public TPublicEntity Update(TPublicEntity entity)
    {
        return Mapper.Map(Service.Update(Mapper.Map(entity)!))!;
    }

    public TPublicEntity Remove(TPublicEntity entity)
    {
        return Mapper.Map(Service.Remove(Mapper.Map(entity)!))!;
    }

    public TPublicEntity Remove(TKey id)
    {
        return Mapper.Map(Service.Remove(id))!;
    }

    public TPublicEntity FirstOrDefault(TKey id, bool noTracking = true)
    {
        return Mapper.Map(Service.FirstOrDefault(id, noTracking))!;
    }

    public IEnumerable<TPublicEntity> GetAll(bool noTracking = true)
    {
        return Service.GetAll(noTracking).Select(e => Mapper.Map(e)!);
    }

    public bool Exists(TKey id)
    {
        return Service.Exists(id);
    }

    public async Task<TPublicEntity?> FirstOrDefaultAsync(TKey id, bool noTracking = true)
    {
        return Mapper.Map(await Service.FirstOrDefaultAsync(id, noTracking));
    }

    public async Task<IEnumerable<TPublicEntity>> GetAllAsync(bool noTracking = true)
    {
        return (await Service.GetAllAsync(noTracking)).Select(e => Mapper.Map(e)!);
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        return await Service.ExistsAsync(id);
    }

    public async Task<TPublicEntity> RemoveAsync(TKey id)
    {
        return Mapper.Map(await Service.RemoveAsync(id))!;
    }
}