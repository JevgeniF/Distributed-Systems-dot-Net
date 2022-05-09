﻿using App.DTO;
using AutoMapper;
using Base.Contracts;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class MovieDbScoreMapper : BaseMapper<MovieDbScore, Domain.MovieDbScore>
{
    public MovieDbScoreMapper(IMapper mapper) : base(mapper)
    {
    }
}