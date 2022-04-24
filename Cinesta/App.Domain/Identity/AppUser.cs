using App.Domain.Common;
using App.Domain.Profile;
using Base.Domain.Identity;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    //username
    //email

    public Guid? PersonId { get; set; } = default!;
    public Person? Person { get; set; }

    public ICollection<UserProfile>? UserProfiles { get; set; }
}