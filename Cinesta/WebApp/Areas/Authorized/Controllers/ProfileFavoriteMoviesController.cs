#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class ProfileFavoriteMoviesController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProfileFavoriteMoviesController> _logger;


    public ProfileFavoriteMoviesController(AppDbContext context, ILogger<ProfileFavoriteMoviesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/ProfileFavoriteMovies
    public async Task<IActionResult> Index()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        return View(await _context.ProfileFavoriteMovies.Include(p => p.MovieDetails)
            .Include(p => p.UserProfile).Where(p => p.UserProfileId == profileId)
            .ToListAsync());
    }

    // GET: Authorized/ProfileFavoriteMovies/Create
    public async Task<ActionResult> Create()
    {
        var profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.Id == profileId);
        var vm = new ProfileFavoriteMovieCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _context.MovieDetails
                    .Where(m => m.AgeRating.AllowedAge <= profile!.Age).ToListAsync())
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
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.Id == profileId);


        if (ModelState.IsValid)
        {
            vm.ProfileFavoriteMovie.UserProfileId = profileId;
            //vm.ProfileFavoriteMovie.Id = Guid.NewGuid();
            _context.ProfileFavoriteMovies.Add(vm.ProfileFavoriteMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new {id = profileId});
        }

        vm.MovieDetailsSelectList = new SelectList((await _context.MovieDetails
                .Where(m => m.AgeRating.AllowedAge <= profile!.Age).ToListAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.ProfileFavoriteMovie.MovieDetailsId);
        return View(vm);
    }


    // GET: Authorized/ProfileFavoriteMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        ViewData["id"] = (await _context.ProfileFavoriteMovies
            .FirstOrDefaultAsync(p => p.Id == id))!.UserProfileId;
        var profileFavoriteMovie = await _context.ProfileFavoriteMovies
            .Include(p => p.MovieDetails)
            .Include(p => p.UserProfile)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (profileFavoriteMovie == null) return NotFound();

        return View(profileFavoriteMovie);
    }

    // POST: Authorized/ProfileFavoriteMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        ViewData["id"] = (await _context.ProfileFavoriteMovies
            .FirstOrDefaultAsync(p => p.Id == id))!.UserProfileId;

        var profileFavoriteMovie = await _context.ProfileFavoriteMovies.FindAsync(id);
        _context.ProfileFavoriteMovies.Remove(profileFavoriteMovie!);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index), new {id = ViewData["id"]});
    }
}