#nullable disable
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Decide if this controller required in API.
namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class ProfileMoviesController : Controller
{
    private readonly IAppBll _bll;

    public ProfileMoviesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/ProfileMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _bll.UserProfile.FirstOrDefaultAsync(profileId);
        return View(await _bll.MovieDetails.IncludeGetByAgeAsync(profile!.Age));
    }
}