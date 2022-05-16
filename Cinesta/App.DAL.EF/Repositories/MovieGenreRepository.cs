using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class MovieGenreRepository : BaseEntityRepository<DTO.MovieGenre, MovieGenre, AppDbContext>,
    IMovieGenreRepository
{
    public MovieGenreRepository(AppDbContext dbContext, IMapper<DTO.MovieGenre, MovieGenre> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<DTO.MovieGenre>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.Genre)
            .Include(m => m.MovieDetails);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }

    public async Task<DTO.MovieGenre?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.Genre)
            .Include(m => m.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
    }
}