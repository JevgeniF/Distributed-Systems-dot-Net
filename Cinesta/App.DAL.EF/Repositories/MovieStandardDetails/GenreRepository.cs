using App.Contracts.DAL.MovieStandardDetails;
using App.Domain.MovieStandardDetails;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.MovieStandardDetails;

public class GenreRepository : BaseEntityRepository<Genre, AppDbContext>, IGenreRepository
{
    public GenreRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}