using App.Domain.Cast;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class CastInMovieCreateEditVM
{
    public CastInMovie CastInMovie { get; set; } = default!;
    public SelectList? CastRoleSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
    public SelectList? PersonSelectList { get; set; }
}