using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Movie;

public class MovieDetailsRepository : BaseEntityRepository<MovieDetails, AppDbContext>, IMovieDetailsRepository
{
    public MovieDetailsRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}