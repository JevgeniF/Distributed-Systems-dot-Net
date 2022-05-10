using App.BLL.DTO;
using App.Contracts.DAL;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IVideoService: IEntityService<Video>, IVideoRepositoryCustom<Video>
{
    
}