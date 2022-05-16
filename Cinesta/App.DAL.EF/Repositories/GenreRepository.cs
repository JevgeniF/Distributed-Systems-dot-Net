using App.Contracts.DAL;
using App.DAL.EF.Mappers;
using App.Domain;
using Base.Contracts;
using Base.Contracts.Mapper;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class GenreRepository : BaseEntityRepository<DTO.Genre, Genre, AppDbContext>, IGenreRepository
{
    public GenreRepository(AppDbContext dbContext, IMapper<DTO.Genre, Genre> mapper) : base(dbContext, mapper)
    {
    }
}