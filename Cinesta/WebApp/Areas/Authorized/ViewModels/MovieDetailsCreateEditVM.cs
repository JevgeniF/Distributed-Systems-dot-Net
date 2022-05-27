#pragma warning disable CS1591
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class MovieDetailsCreateEditVm
{
    public MovieDetails MovieDetails { get; set; } = default!;

    public SelectList? AgeRatingSelectList { get; set; }
    public SelectList? MovieTypeSelectList { get; set; }
}