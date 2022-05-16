using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class MovieTypeRepository : BaseEntityRepository<DTO.MovieType, MovieType, AppDbContext>, IMovieTypeRepository
{
    public MovieTypeRepository(AppDbContext dbContext, IMapper<DTO.MovieType, MovieType> mapper) : base(dbContext,
        mapper)
    {
    }
}