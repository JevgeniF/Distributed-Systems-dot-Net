using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class UserRatingMapper : BaseMapper<UserRating, Domain.UserRating>
{
    public UserRatingMapper(IMapper mapper) : base(mapper)
    {
    }
}