#pragma warning disable CS1591
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieDbScoreCreateEditVM
{
    public MovieDbScore MovieDbScore { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}