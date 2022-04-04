﻿using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class MovieGenreCreateEditVM
{
    public MovieGenre MovieGenre { get; set; } = default!;
    
    public SelectList? GenreSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
}