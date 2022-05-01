using App.Domain.Movie;
using App.Domain.Profile;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Profile;

public interface IProfileMovieRepository : IEntityRepository<ProfileMovie>
{
    Task<IEnumerable<MovieDetails>> GetAllByProfileAgeAsync(int age, bool noTracking = true);
}