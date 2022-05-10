using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class ProfileFavoriteMovieCreateEditVM
{
    public ProfileFavoriteMovie ProfileFavoriteMovie { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}