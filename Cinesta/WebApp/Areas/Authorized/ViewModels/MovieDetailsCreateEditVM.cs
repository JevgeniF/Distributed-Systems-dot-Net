﻿#pragma warning disable CS1591
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieDetailsCreateEditVM
{
    public MovieDetails MovieDetails { get; set; } = default!;

    public SelectList? AgeRatingSelectList { get; set; }
    public SelectList? MovieTypeSelectList { get; set; }
}