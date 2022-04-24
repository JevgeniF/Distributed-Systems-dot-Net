using App.Contracts.DAL.Cast;
using App.Contracts.DAL.MovieStandardDetails;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUOW : IUnitOfWork
{
    ICastRoleRepository CastRole { get; }
    IAgeRatingRepository AgeRating { get; }
    IGenreRepository Genre { get; }
}