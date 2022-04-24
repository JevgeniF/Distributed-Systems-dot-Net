using App.Contracts.DAL.MovieStandardDetails;
using App.Domain.MovieStandardDetails;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.MovieStandardDetails;

public class MovieTypeRepository: BaseEntityRepository<MovieType, AppDbContext>, IMovieTypeRepository
{
    public MovieTypeRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}