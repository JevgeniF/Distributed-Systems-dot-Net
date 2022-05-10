﻿using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class GenreService : BaseEntityService<Genre, App.DAL.DTO.Genre, IGenreRepository>, IGenreService
{
    public GenreService(IGenreRepository repository, IMapper<Genre, DAL.DTO.Genre> mapper) : base(repository, mapper)
    {
    }
}