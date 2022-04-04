using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.ViewModels;

public class MovieDbScoreCreateEditVM
{
    public MovieDbScore MovieDbScore { get; set; } = default!;
    
    public SelectList? MovieDetailsSelectList { get; set; }

}