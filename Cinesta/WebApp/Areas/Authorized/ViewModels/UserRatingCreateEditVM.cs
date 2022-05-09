using App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class UserRatingCreateEditVM
{
    public UserRating UserRating { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}