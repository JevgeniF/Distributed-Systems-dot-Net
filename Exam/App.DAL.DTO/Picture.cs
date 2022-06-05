using Base.Domain;

namespace App.DAL.DTO;

public class Picture: DomainEntityId
{
    public string PictureUri { get; set; } = default!;
}