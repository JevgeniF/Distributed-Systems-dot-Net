#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class ProfileFavoriteMoviesController : Controller
{
    private readonly ILogger<ProfileFavoriteMoviesController> _logger;
    private readonly IAppPublic _public;


    public ProfileFavoriteMoviesController(IAppPublic appPublic, ILogger<ProfileFavoriteMoviesController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/ProfileFavoriteMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        return View(await _public.ProfileFavoriteMovie.IncludeGetAllByProfileIdAsync(profileId));
    }

    // GET: Authorized/ProfileFavoriteMovies/Create
    public async Task<ActionResult> Create()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _public.UserProfile.FirstOrDefaultAsync(profileId);
        var vm = new ProfileFavoriteMovieCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _public.MovieDetails.IncludeGetByAgeAsync(profile!.Age))
                .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
        };

        return View(vm);
    }

    // POST: ProfileFavoriteMovies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfileFavoriteMovieCreateEditVM vm)
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _public.UserProfile.FirstOrDefaultAsync(profileId);


        if (ModelState.IsValid)
        {
            vm.ProfileFavoriteMovie.UserProfileId = profileId;
            //vm.ProfileFavoriteMovie.Id = Guid.NewGuid();
            _public.ProfileFavoriteMovie.Add(vm.ProfileFavoriteMovie);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {id = profileId});
        }

        vm.MovieDetailsSelectList = new SelectList((await _public.MovieDetails.IncludeGetByAgeAsync(profile!.Age))
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.ProfileFavoriteMovie.MovieDetailsId);
        return View(vm);
    }


    // GET: Authorized/ProfileFavoriteMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var profileFavoriteMovie = await _public.ProfileFavoriteMovie.IncludeFirstOrDefaultAsync(id.Value);
        if (profileFavoriteMovie == null) return NotFound();
        ViewData["id"] = profileFavoriteMovie.UserProfileId;
        return View(profileFavoriteMovie);
    }

    // POST: Authorized/ProfileFavoriteMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        ViewData["id"] = (await _public.ProfileFavoriteMovie.FirstOrDefaultAsync(id))!.UserProfileId;

        await _public.ProfileFavoriteMovie.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new {id = ViewData["id"]});
    }
}