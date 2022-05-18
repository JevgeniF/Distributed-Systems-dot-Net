#pragma warning disable CS1591
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieDbScore = App.Domain.MovieDbScore;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieDbScoreCreateEditVM
{
    public MovieDbScore MovieDbScore { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}