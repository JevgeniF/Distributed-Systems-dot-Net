using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UserProfileMapper : BaseMapper<UserProfile, DAL.DTO.UserProfile>
{
    public UserProfileMapper(IMapper mapper) : base(mapper)
    {
    }
}