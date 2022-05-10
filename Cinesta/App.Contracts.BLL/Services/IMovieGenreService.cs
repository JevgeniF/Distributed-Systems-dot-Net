using Base.Contracts.BLL;
using App.BLL.DTO;
using App.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IMovieGenreService : IEntityService<MovieGenre>, IMovieGenreRepositoryCustom<MovieGenre>
{
}