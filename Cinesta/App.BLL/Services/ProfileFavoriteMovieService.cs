using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts.Mapper;

namespace App.BLL.Services;

public class ProfileFavoriteMovieService :
    BaseEntityService<ProfileFavoriteMovie, DAL.DTO.ProfileFavoriteMovie, IProfileFavoriteMovieRepository>,
    IProfileFavoriteMovieService
{
    public ProfileFavoriteMovieService(IProfileFavoriteMovieRepository repository,
        IMapper<ProfileFavoriteMovie, DAL.DTO.ProfileFavoriteMovie> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true)
    {
        return (await Repository.IncludeGetAllByProfileIdAsync(profileId, noTracking)).Select(p => Mapper.Map(p)!);
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Repository.IncludeGetAllAsync(noTracking)).Select(p => Mapper.Map(p)!);
    }

    public async Task<ProfileFavoriteMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Repository.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}