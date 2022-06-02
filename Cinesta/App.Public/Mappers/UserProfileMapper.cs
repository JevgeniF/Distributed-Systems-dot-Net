using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class UserProfileMapper : BaseMapper<UserProfile, BLL.DTO.UserProfile>
{
    public UserProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}