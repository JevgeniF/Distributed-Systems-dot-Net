using App.BLL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class GenreMapper : BaseMapper<Genre, DAL.DTO.Genre>
{
    public GenreMapper(IMapper mapper) : base(mapper)
    {
    }
}