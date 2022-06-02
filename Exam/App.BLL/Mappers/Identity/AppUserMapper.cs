using App.BLL.DTO.Identity;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers.Identity;

public class AppUserMapper : BaseMapper<AppUser, DAL.DTO.Identity.AppUser>
{
    public AppUserMapper(IMapper mapper) : base(mapper)
    {
    }
}