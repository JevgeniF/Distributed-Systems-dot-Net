using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class GenreRepository : BaseEntityRepository<Genre, Domain.Genre, AppDbContext>, IGenreRepository
{
    public GenreRepository(AppDbContext dbContext, IMapper<Genre, Domain.Genre> mapper) : base(dbContext, mapper)
    {
    }
}