using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using AutoMapper;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using CastInMovie = App.DTO.CastInMovie;

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
        query = query.Include(c => c.CastRole).Include(c => c.Persons);
        return (await query.ToListAsync()).Select(c => Mapper.Map(c)!);
    }
    
    public async Task<CastInMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(c => c.CastRole).Include(c => c.Persons);
        return Mapper.Map(await query.FirstOrDefaultAsync(c => c.Id == id));
    }
}