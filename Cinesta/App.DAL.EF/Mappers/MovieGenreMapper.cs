﻿using App.DAL.DTO;
using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MovieGenreMapper : BaseMapper<MovieGenre, Domain.MovieGenre>
{
    public MovieGenreMapper(IMapper mapper) : base(mapper)
    {
    }
}