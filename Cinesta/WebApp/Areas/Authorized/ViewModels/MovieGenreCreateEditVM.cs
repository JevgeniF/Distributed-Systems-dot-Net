#pragma warning disable CS1591
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieGenreCreateEditVm
{
    public MovieGenre MovieGenre { get; set; } = default!;

    public SelectList? GenreSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
}