using App.Contracts.DAL.Movie;
using App.Domain.Movie;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Movie;

public class VideoRepository : BaseEntityRepository<Video, AppDbContext>, IVideoRepository
{
    public VideoRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}