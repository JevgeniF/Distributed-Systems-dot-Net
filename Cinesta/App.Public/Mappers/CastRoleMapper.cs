using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class CastRoleMapper : BaseMapper<CastRole, BLL.DTO.CastRole>
{
    public CastRoleMapper(IMapper mapper) : base(mapper)
    {
    }
}