using App.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieDetailsCreateEditVM
{
    public MovieDetails MovieDetails { get; set; } = default!;

    public SelectList? AgeRatingSelectList { get; set; }
    public SelectList? MovieTypeSelectList { get; set; }
}