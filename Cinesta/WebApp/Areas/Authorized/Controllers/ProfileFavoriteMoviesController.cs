#nullable disable
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Movie;
using App.Domain.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class ProfileFavoriteMoviesController : Controller
{
    private readonly IAppUOW _uow;
    private Guid _profileId;

    public ProfileFavoriteMoviesController(IAppUOW uow)
    {
        
        _uow = uow;
    }

    // GET: Admin/ProfileFavoriteMovies
    public async Task<IActionResult> Index()
    {
        _profileId = Guid.Parse(RouteData.Values["id"]!.ToString()!);
        return View(await _uow.ProfileFavoriteMovie.GetAllByProfileIdAsync(_profileId));
    }

    // GET: Admin/ProfileFavoriteMovies/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var profileFavoriteMovie = await _uow.ProfileFavoriteMovie.FirstOrDefaultAsync(id.Value);
        if (profileFavoriteMovie == null) return NotFound();

        return View(profileFavoriteMovie);
    }

    // GET: Admin/ProfileFavoriteMovies/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ProfileFavoriteMovies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProfileFavoriteMovie profileFavoriteMovie)
    {
        if (ModelState.IsValid)
        {
            profileFavoriteMovie.UserProfileId = _profileId;
            profileFavoriteMovie.Id = Guid.NewGuid();
            _uow.ProfileFavoriteMovie.Add(profileFavoriteMovie);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View();
    }

    // GET: ProfileFavoriteMovies/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var profileFavoriteMovie = await _uow.ProfileFavoriteMovie.FirstOrDefaultAsync(id.Value);
        if (profileFavoriteMovie == null) return NotFound();
        
        return View(profileFavoriteMovie);
    }

    // POST: ProfileFavoriteMovies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProfileFavoriteMovie profileFavoriteMovie)
    {
        if (id != profileFavoriteMovie.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _uow.ProfileFavoriteMovie.Update(profileFavoriteMovie);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProfileFavoriteMovieExists(profileFavoriteMovie.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
        return View(profileFavoriteMovie);
    }


    // GET: Admin/ProfileFavoriteMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var profileFavoriteMovie = await _uow.ProfileFavoriteMovie.FirstOrDefaultAsync(id.Value);
        if (profileFavoriteMovie == null) return NotFound();

        return View(profileFavoriteMovie);
    }

    // POST: Admin/ProfileFavoriteMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.ProfileFavoriteMovie.FirstOrDefaultAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> ProfileFavoriteMovieExists(Guid id)
    {
        return await _uow.ProfileFavoriteMovie.ExistsAsync(id);
    }
}