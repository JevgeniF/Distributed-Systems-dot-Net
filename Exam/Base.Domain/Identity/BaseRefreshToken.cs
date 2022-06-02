using System.ComponentModel.DataAnnotations;

namespace Base.Domain.Identity;

public class BaseRefreshToken : DomainEntityId
{
    [StringLength(36, MinimumLength = 36)] public string Token { get; set; } = Guid.NewGuid().ToString();
    public DateTime TokenExpirationDateTime { get; set; } = DateTime.UtcNow.AddDays(7);

    [StringLength(36, MinimumLength = 36)] public string? PreviousToken { get; set; }

    public DateTime PreviousTokenExpirationDateTime { get; set; }
}