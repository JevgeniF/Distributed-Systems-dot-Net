using App.Public.DTO.v1;
using AutoMapper;
using Base.DAL;

namespace App.Public.Mappers;

public class GenreMapper : BaseMapper<Genre, BLL.DTO.Genre>
{
    public GenreMapper(IMapper mapper) : base(mapper)
    {
    }
}