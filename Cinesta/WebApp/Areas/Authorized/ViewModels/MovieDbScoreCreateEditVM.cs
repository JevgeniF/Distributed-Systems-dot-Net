using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieDbScoreCreateEditVM
{
    public MovieDbScore MovieDbScore { get; set; } = default!;

    public SelectList? MovieDetailsSelectList { get; set; }
}