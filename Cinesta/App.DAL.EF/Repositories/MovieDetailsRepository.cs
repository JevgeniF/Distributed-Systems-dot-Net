using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class MovieDetailsRepository : BaseEntityRepository<DTO.MovieDetails, MovieDetails, AppDbContext>,
    IMovieDetailsRepository
{
    public MovieDetailsRepository(AppDbContext dbContext, IMapper<DTO.MovieDetails, MovieDetails> mapper) : base(
        dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DTO.MovieDetails>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.AgeRating)
            .Include(m => m.MovieType);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }
    
    public async Task<DTO.MovieDetails?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.AgeRating)
            .Include(m => m.MovieType);
        return Mapper.Map(await query.FirstOrDefaultAsync(m => m.Id == id));
    }

    public async Task<IEnumerable<DTO.MovieDetails>> IncludeGetByAgeAsync(int age, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(m => m.AgeRating)
            .Include(m => m.MovieType)
            .Where(m => m.AgeRating!.AllowedAge <= age);
        return (await query.ToListAsync()).Select(m => Mapper.Map(m)!);
    }
}