using App.DAL.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UserProfileMapper : BaseMapper<UserProfile, Domain.UserProfile>
{
    public UserProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}