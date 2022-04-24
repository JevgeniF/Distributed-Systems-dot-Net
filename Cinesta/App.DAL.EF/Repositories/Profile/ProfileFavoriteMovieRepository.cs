using App.Contracts.DAL.Profile;
using App.Domain.Profile;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Profile;

public class ProfileFavoriteMovieRepository: BaseEntityRepository<ProfileFavoriteMovie, AppDbContext>, IProfileFavoriteMovieRepository
{
    public ProfileFavoriteMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}