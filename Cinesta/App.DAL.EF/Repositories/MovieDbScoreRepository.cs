using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class MovieDbScoreRepository : BaseEntityRepository<DTO.MovieDbScore, MovieDbScore, AppDbContext>,
    IMovieDbScoreRepository
{
    public MovieDbScoreRepository(AppDbContext dbContext, IMapper<DTO.MovieDbScore, MovieDbScore> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DTO.MovieDbScore>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query.Include(m => m.MovieDetails);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }
    
    public async Task<DTO.MovieDbScore?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query.Include(m => m.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
    }
}