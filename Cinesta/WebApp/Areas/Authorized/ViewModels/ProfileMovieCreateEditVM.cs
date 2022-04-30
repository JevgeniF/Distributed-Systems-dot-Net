using App.Domain.Profile;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class ProfileMovieCreateEditVM
{
    public ProfileMovie ProfileMovie { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
    public SelectList? UserProfileSelectList { get; set; }
}