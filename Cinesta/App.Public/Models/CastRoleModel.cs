using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class CastRoleModel : BaseEntityModel<CastRole, BLL.DTO.CastRole, ICastRoleService>,
    ICastRoleModel
{
    public CastRoleModel(ICastRoleService service, IMapper<CastRole, BLL.DTO.CastRole> mapper) : base(
        service, mapper)
    {
    }
}