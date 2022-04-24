using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Movie;

public class MovieDBScoreRepository: BaseEntityRepository<MovieDbScore, AppDbContext>, IMovieDBScoreRepository
{
    public MovieDBScoreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}