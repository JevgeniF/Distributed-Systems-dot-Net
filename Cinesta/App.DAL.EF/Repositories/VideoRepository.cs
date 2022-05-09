using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Microsoft.EntityFrameworkCore;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class VideoRepository : BaseEntityRepository<DTO.Video, Video, AppDbContext>, IVideoRepository
{
    public VideoRepository(AppDbContext dbContext, IMapper<DTO.Video, Video> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<DTO.Video>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(v => v.MovieDetails);
        return (await query.ToListAsync()).Select(v => Mapper.Map(v)!);
    }

    public async Task<DTO.Video?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(v => v.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(v => v.Id == id));
    }
}