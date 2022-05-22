#pragma warning disable CS1591
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieGenreCreateEditVM
{
    public MovieGenre MovieGenre { get; set; } = default!;

    public SelectList? GenreSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
}