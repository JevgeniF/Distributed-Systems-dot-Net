#pragma warning disable CS1591
using App.BLL.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Authorized.ViewModels;

public class CastInMovieCreateEditVm
{
    public CastInMovie CastInMovie { get; set; } = default!;
    public SelectList? CastRoleSelectList { get; set; }
    public SelectList? MovieDetailsSelectList { get; set; }
    public SelectList? PersonSelectList { get; set; }
}