using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class CastRoleService : BaseEntityService<CastRole, DAL.DTO.CastRole, ICastRoleRepository>, ICastRoleService
{
    public CastRoleService(ICastRoleRepository repository, IMapper<CastRole, DAL.DTO.CastRole> mapper) : base(
        repository, mapper)
    {
    }
}