using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.ViewModels;

public class VideoCreateEditVM
{
    public Video Video { get; set; } = default!;
    
    public SelectList? MovieDetailsSelectList { get; set; }
}