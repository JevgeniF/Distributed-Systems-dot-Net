using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class UserRatingMapper : BaseMapper<UserRating, BLL.DTO.UserRating>
{
    public UserRatingMapper(IMapper mapper) : base(mapper)
    {
    }
}