using App.Domain.Movie;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Movie;

public interface IVideoRepository : IEntityRepository<Video>
{
}