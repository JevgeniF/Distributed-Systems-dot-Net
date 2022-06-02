using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class ProfileMovieRepository : BaseEntityRepository<ProfileMovie, Domain.ProfileMovie, AppDbContext>,
    IProfileMovieRepository
{
    public ProfileMovieRepository(AppDbContext dbContext, IMapper<ProfileMovie, Domain.ProfileMovie> mapper) : base(
        dbContext, mapper)
    {
    }
}