#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class ProfileFavoriteMoviesController : Controller
{
    private readonly IAppBll _bll;


    public ProfileFavoriteMoviesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/ProfileFavoriteMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        return View(await _bll.ProfileFavoriteMovie.IncludeGetAllByProfileIdAsync(profileId));
    }

    // GET: Admin/ProfileFavoriteMovies/Create
    public async Task<ActionResult> Create()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _bll.UserProfile.FirstOrDefaultAsync(profileId);
        var vm = new ProfileFavoriteMovieCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.IncludeGetByAgeAsync(profile!.Age))
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
        var profile = await _bll.UserProfile.FirstOrDefaultAsync(profileId);

        if (ModelState.IsValid)
        {
            vm.ProfileFavoriteMovie.UserProfileId = profileId;
            vm.ProfileFavoriteMovie.Id = Guid.NewGuid();
            _bll.ProfileFavoriteMovie.Add(vm.ProfileFavoriteMovie);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {id = profileId});
        }

        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.IncludeGetByAgeAsync(profile!.Age))
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.ProfileFavoriteMovie.MovieDetailsId);
        return View(vm);
    }


    // GET: Admin/ProfileFavoriteMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        ViewData["id"] = (await _bll.ProfileFavoriteMovie.FirstOrDefaultAsync(id.Value))!.UserProfileId;
        var profileFavoriteMovie = await _bll.ProfileFavoriteMovie.IncludeFirstOrDefaultAsync(id.Value);
        if (profileFavoriteMovie == null) return NotFound();

        return View(profileFavoriteMovie);
    }

    // POST: Admin/ProfileFavoriteMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        ViewData["id"] = (await _bll.ProfileFavoriteMovie.FirstOrDefaultAsync(id))!.UserProfileId;

        await _bll.ProfileFavoriteMovie.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new {id = ViewData["id"]});
    }
}