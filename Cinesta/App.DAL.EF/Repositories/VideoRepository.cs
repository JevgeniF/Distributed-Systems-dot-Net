using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class VideoRepository : BaseEntityRepository<Video, Domain.Video, AppDbContext>, IVideoRepository
{
    public VideoRepository(AppDbContext dbContext, IMapper<Video, Domain.Video> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Video>> IncludeGetAllAsync(bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(v => v.MovieDetails);
        return (await query.ToListAsync()).Select(v => Mapper.Map(v)!);
    }

    public async Task<Video?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        query = query.Include(v => v.MovieDetails);
        return Mapper.Map(await query.FirstOrDefaultAsync(v => v.Id == id));
    }
}