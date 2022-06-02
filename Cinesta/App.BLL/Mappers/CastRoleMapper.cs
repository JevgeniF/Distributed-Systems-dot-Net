using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class CastRoleMapper : BaseMapper<CastRole, DAL.DTO.CastRole>
{
    public CastRoleMapper(IMapper mapper) : base(mapper)
    {
    }
}