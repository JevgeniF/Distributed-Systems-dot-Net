using App.Contracts.BLL.Services;
using App.Contracts.Public.Models;
using App.Public.DTO.v1;
using Base.Contracts.Mapper;
using Base.Public;

namespace App.Public.Models;

public class ProfileFavoriteMovieModel :
    BaseEntityModel<ProfileFavoriteMovie, BLL.DTO.ProfileFavoriteMovie, IProfileFavoriteMovieService>,
    IProfileFavoriteMovieModel
{
    public ProfileFavoriteMovieModel(IProfileFavoriteMovieService service,
        IMapper<ProfileFavoriteMovie, BLL.DTO.ProfileFavoriteMovie> mapper) : base(
        service, mapper)
    {
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> IncludeGetAllByProfileIdAsync(Guid profileId,
        bool noTracking = true)
    {
        return (await Service.IncludeGetAllByProfileIdAsync(profileId, noTracking)).Select(p => Mapper.Map(p)!);
    }

    public async Task<IEnumerable<ProfileFavoriteMovie>> IncludeGetAllAsync(bool noTracking = true)
    {
        return (await Service.IncludeGetAllAsync(noTracking)).Select(p => Mapper.Map(p)!);
    }

    public async Task<ProfileFavoriteMovie?> IncludeFirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        return Mapper.Map(await Service.IncludeFirstOrDefaultAsync(id, noTracking));
    }
}