#pragma warning disable CS1591
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class ProfileFavoriteMovieCreateEditVm
{
    public ProfileFavoriteMovie ProfileFavoriteMovie { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}