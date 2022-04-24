using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Movie;

public class MovieGenreRepository: BaseEntityRepository<MovieGenre, AppDbContext>, IMovieGenreRepository
{
    public MovieGenreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}