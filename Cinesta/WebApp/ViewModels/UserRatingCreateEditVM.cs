using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class UserRatingCreateEditVM
{
    public UserRating UserRating { get; set; } = default!;
    
    public SelectList? AppUserSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
}