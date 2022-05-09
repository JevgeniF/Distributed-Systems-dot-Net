using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class CastRoleMapper : BaseMapper<CastRole, App.Domain.CastRole>
{
    public CastRoleMapper(IMapper mapper) : base(mapper)
    {
    }
}