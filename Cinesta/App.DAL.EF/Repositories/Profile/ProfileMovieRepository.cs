using App.Contracts.DAL.Profile;
using App.Domain.Profile;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories.Profile;

public class ProfileMovieRepository : BaseEntityRepository<ProfileMovie, AppDbContext>, IProfileMovieRepository
{
    public ProfileMovieRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}