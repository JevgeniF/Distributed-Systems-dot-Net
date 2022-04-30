#nullable disable
using App.DAL.EF;
using App.Domain.Movie;
using App.Domain.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
public class ProfileMoviesController : Controller
{
    private readonly AppDbContext _context;

    public ProfileMoviesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/ProfileMovies
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.ProfileMovies.Include(p => p.MovieDetails).Include(p => p.UserProfile);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/ProfileMovies/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var profileMovie = await _context.ProfileMovies
            .Include(p => p.MovieDetails)
            .Include(p => p.UserProfile)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (profileMovie == null) return NotFound();

        return View(profileMovie);
    }

    // GET: Admin/ProfileMovies/Create
    public async Task<ActionResult> Create()
    {
        var vm = new ProfileMovieCreateEditVM();
        vm.MovieDetailsSelectList = new SelectList(
            await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title));
        vm.UserProfileSelectList = new SelectList(
            await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
            nameof(UserProfile.Id), nameof(UserProfile.Name));
        return View(vm);
    }

    // POST: ProfileMovies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfileMovieCreateEditVM vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.ProfileMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList(
            await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title),
            vm.ProfileMovie.MovieDetailsId);
        vm.UserProfileSelectList = new SelectList(
            await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
            nameof(UserProfile.Id), nameof(UserProfile.Name),
            vm.ProfileMovie.UserProfileId);
        return View(vm);
    }

    // GET: ProfileMovies/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var profileMovie = await _context.ProfileMovies.FindAsync(id);
        if (profileMovie == null) return NotFound();
        var vm = new ProfileMovieCreateEditVM();
        vm.ProfileMovie = profileMovie;
        vm.MovieDetailsSelectList = new SelectList(
            await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title),
            vm.ProfileMovie.MovieDetailsId);
        vm.UserProfileSelectList = new SelectList(
            await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
            nameof(UserProfile.Id), nameof(UserProfile.Name),
            vm.ProfileMovie.UserProfileId);
        return View(vm);
    }

    // POST: ProfileMovies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProfileMovie profileMovie)
    {
        if (id != profileMovie.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(profileMovie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileMovieExists(profileMovie.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new ProfileMovieCreateEditVM();
        vm.ProfileMovie = profileMovie;
        vm.MovieDetailsSelectList = new SelectList(
            await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title),
            vm.ProfileMovie.MovieDetailsId);
        vm.UserProfileSelectList = new SelectList(
            await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
            nameof(UserProfile.Id), nameof(UserProfile.Name),
            vm.ProfileMovie.UserProfileId);
        return View(vm);
    }


    // GET: Admin/ProfileMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var profileMovie = await _context.ProfileMovies
            .Include(p => p.MovieDetails)
            .Include(p => p.UserProfile)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (profileMovie == null) return NotFound();

        return View(profileMovie);
    }

    // POST: Admin/ProfileMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var profileMovie = await _context.ProfileMovies.FindAsync(id);
        _context.ProfileMovies.Remove(profileMovie);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProfileMovieExists(Guid id)
    {
        return _context.ProfileMovies.Any(e => e.Id == id);
    }
}