using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CastRoleMapper : BaseMapper<CastRole, Domain.CastRole>
{
    public CastRoleMapper(IMapper mapper) : base(mapper)
    {
    }
}