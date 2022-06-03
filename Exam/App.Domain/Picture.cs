using Base.Domain;

namespace App.Domain;

public class Picture: DomainEntityMetaId
{
    public string PictureUri { get; set; } = default!;
}