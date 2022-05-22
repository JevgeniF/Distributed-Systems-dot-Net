#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Decide if this controller required in API.
namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class ProfileMoviesController : Controller
{
    private readonly ILogger<ProfileMoviesController> _logger;
    private readonly IAppPublic _public;

    public ProfileMoviesController(IAppPublic appPublic, ILogger<ProfileMoviesController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/ProfileMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _public.UserProfile.FirstOrDefaultAsync(profileId);
        return View(await _public.MovieDetails.IncludeGetByAgeAsync(profile!.Age));
    }
}