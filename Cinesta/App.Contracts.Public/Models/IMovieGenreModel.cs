using App.Contracts.DAL;
using App.Public.DTO.v1;
using Base.Contracts.Public;

namespace App.Contracts.Public.Models;

public interface IMovieGenreModel : IEntityModel<MovieGenre>, IMovieGenreRepositoryCustom<MovieGenre>
{
}