using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ProfileMovieRepository : BaseEntityRepository<DTO.ProfileMovie, ProfileMovie, AppDbContext>,
    IProfileMovieRepository
{
    public ProfileMovieRepository(AppDbContext dbContext, IMapper<DTO.ProfileMovie, ProfileMovie> mapper) : base(
        dbContext, mapper)
    {
    }
}