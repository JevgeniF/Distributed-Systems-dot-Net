#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// TODO: Decide if this controller required in API.
namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class ProfileMoviesController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProfileMoviesController> _logger;

    public ProfileMoviesController(AppDbContext context, ILogger<ProfileMoviesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/ProfileMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.Id == profileId);
        return View(await _context.MovieDetails
            .Include(m => m.AgeRating)
            .Include(m => m.MovieType)
            .Where(m => m.AgeRating.AllowedAge <= profile.Age)
            .ToListAsync());
    }
}