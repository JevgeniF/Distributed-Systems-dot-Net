using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntityMetaId : DomainEntityMetaId<Guid>, IDomainEntityId
{
}

public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey>, IDomainEntityMeta where TKey : IEquatable<TKey>
{
    [MaxLength(50)] public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(50)] public string? UpdatedBy { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}