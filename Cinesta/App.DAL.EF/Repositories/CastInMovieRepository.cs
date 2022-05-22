using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class CastInMovieRepository : BaseEntityRepository<CastInMovie, Domain.CastInMovie,
    AppDbContext>, ICastInMovieRepository
{
    public CastInMovieRepository(AppDbContext dbContext, IMapper<CastInMovie, Domain.CastInMovie> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<CastInMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(c => c.CastRole).Include(c => c.Persons).Include(c => c.MovieDetails);
        return (await query.ToListAsync()).Select(c => Mapper.Map(c)!);
    }

    public async Task<CastInMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(c => c.CastRole).Include(c => c.Persons).Include(c => c.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(c => c.Id == id));
    }
}