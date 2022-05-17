using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MovieGenreRepository : BaseEntityRepository<MovieGenre, Domain.MovieGenre, AppDbContext>,
    IMovieGenreRepository
{
    public MovieGenreRepository(AppDbContext dbContext, IMapper<MovieGenre, Domain.MovieGenre> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<MovieGenre>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.Genre)
            .Include(m => m.MovieDetails);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieGenre?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.Genre)
            .Include(m => m.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
    }
}