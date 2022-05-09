using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class GenreMapper : BaseMapper<Genre, Domain.Genre>
{
    public GenreMapper(IMapper mapper) : base(mapper)
    {
    }
}