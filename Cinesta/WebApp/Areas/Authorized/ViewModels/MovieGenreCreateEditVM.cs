#pragma warning disable CS1591
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieGenre = App.Domain.MovieGenre;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieGenreCreateEditVM
{
    public MovieGenre MovieGenre { get; set; } = default!;

    public SelectList? GenreSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
}