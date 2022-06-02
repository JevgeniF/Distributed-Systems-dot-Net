using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MovieDbScoreRepository : BaseEntityRepository<MovieDbScore, Domain.MovieDbScore, AppDbContext>,
    IMovieDbScoreRepository
{
    public MovieDbScoreRepository(AppDbContext dbContext, IMapper<MovieDbScore, Domain.MovieDbScore> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<MovieDbScore>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query.Include(m => m.MovieDetails);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieDbScore?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query.Include(m => m.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
    }
    
    public async Task<MovieDbScore?> GetMovieDbScoresForMovie(Guid movieId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query.Include(m => m.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.MovieDetailsId == movieId));
    }
}