﻿using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class GenreMapper : BaseMapper<Genre, Domain.Genre>
{
    public GenreMapper(IMapper mapper) : base(mapper)
    {
    }
}