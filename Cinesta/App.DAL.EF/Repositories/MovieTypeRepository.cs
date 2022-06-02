using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class MovieTypeRepository : BaseEntityRepository<MovieType, Domain.MovieType, AppDbContext>, IMovieTypeRepository
{
    public MovieTypeRepository(AppDbContext dbContext, IMapper<MovieType, Domain.MovieType> mapper) : base(dbContext,
        mapper)
    {
    }
}