using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class MovieDetailsCreateEditVM
{
    public MovieDetails MovieDetails { get; set; } = default!;
    
    public SelectList? AgeRatingSelectList { get; set; }
    public SelectList? MovieTypeSelectList { get; set; }
}