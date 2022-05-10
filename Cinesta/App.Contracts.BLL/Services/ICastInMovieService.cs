using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface ICastInMovieService: IEntityService<App.BLL.DTO.CastInMovie>, ICastInMovieRepositoryCustom<App.BLL.DTO.CastInMovie>
{
    
}