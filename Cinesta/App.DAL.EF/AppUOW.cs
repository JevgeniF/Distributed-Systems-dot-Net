using App.Contracts.DAL;
using App.Contracts.DAL.Cast;
using App.Contracts.DAL.MovieStandardDetails;
using App.DAL.EF.Repositories.Cast;
using App.DAL.EF.Repositories.MovieStandardDetails;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUOW
{
    public AppUOW(AppDbContext dbContext) : base(dbContext)
    {
    }

    // cast
    private ICastRoleRepository? _castRole;
    public virtual ICastRoleRepository CastRole => _castRole ??= new CastRoleRepository(UOWDbContext);

    //movie standard details
    private IAgeRatingRepository? _ageRating;
    public virtual IAgeRatingRepository AgeRating => _ageRating ??= new AgeRatingRepository(UOWDbContext);
    private IGenreRepository? _genre;
    public virtual IGenreRepository Genre => _genre ??= new GenreRepository(UOWDbContext);
}