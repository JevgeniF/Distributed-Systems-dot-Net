#pragma warning disable CS1591
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProfileFavoriteMovie = App.Domain.ProfileFavoriteMovie;

namespace WebApp.Areas.Authorized.ViewModels;

public class ProfileFavoriteMovieCreateEditVM
{
    public ProfileFavoriteMovie ProfileFavoriteMovie { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}