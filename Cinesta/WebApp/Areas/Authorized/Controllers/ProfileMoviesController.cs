#nullable disable
using App.Contracts.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class ProfileMoviesController : Controller
{
    private readonly IAppUOW _uow;

    public ProfileMoviesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/ProfileMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _uow.UserProfile.FirstOrDefaultAsync(profileId);
        return View(await _uow.ProfileMovie.GetAllByProfileAgeAsync(profile!.Age));
    }
}