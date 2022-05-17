using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class MovieDetailsRepository : BaseEntityRepository<MovieDetails, Domain.MovieDetails, AppDbContext>,
    IMovieDetailsRepository
{
    public MovieDetailsRepository(AppDbContext dbContext, IMapper<MovieDetails, Domain.MovieDetails> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<MovieDetails>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.AgeRating)
            .Include(m => m.MovieType);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }

    public async Task<MovieDetails?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.AgeRating)
            .Include(m => m.MovieType);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
    }

    public async Task<IEnumerable<MovieDetails>> IncludeGetByAgeAsync(int age, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.AgeRating)
            .Include(m => m.MovieType)
            .Where(m => m.AgeRating!.AllowedAge <= age);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }
}