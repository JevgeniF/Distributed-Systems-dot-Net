using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class UserRatingMapper : BaseMapper<UserRating, DAL.DTO.UserRating>
{
    public UserRatingMapper(IMapper mapper) : base(mapper)
    {
    }
}