#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Movie;
using App.Domain.Profile;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class ProfileFavoriteMoviesController : Controller
    {
        private readonly AppDbContext _context;

        public ProfileFavoriteMoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ProfileFavoriteMovies
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ProfileFavoriteMovies.Include(p => p.MovieDetails).Include(p => p.UserProfile);
            return View(await appDbContext.ToListAsync());
        }

        // GET: ProfileFavoriteMovies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileFavoriteMovie = await _context.ProfileFavoriteMovies
                .Include(p => p.MovieDetails)
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileFavoriteMovie == null)
            {
                return NotFound();
            }

            return View(profileFavoriteMovie);
        }

        // GET: ProfileFavoriteMovies/Create
        public async Task<ActionResult> Create()
        {
            var vm = new ProfileFavoriteMovieCreateEditVM();
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title));
            vm.UserProfileSelectList = new SelectList(
                await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
                nameof(UserProfile.Id), nameof(UserProfile.Name));
            return View(vm);
        }

        // POST: ProfileFavoriteMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProfileFavoriteMovieCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.ProfileFavoriteMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title),
                vm.ProfileFavoriteMovie.MovieDetailsId);
            vm.UserProfileSelectList = new SelectList(
                await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
                nameof(UserProfile.Id), nameof(UserProfile.Name),
                vm.ProfileFavoriteMovie.UserProfileId);
            return View(vm);
        }

        // GET: ProfileFavoriteMovies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileFavoriteMovie = await _context.ProfileFavoriteMovies.FindAsync(id);
            if (profileFavoriteMovie == null)
            {
                return NotFound();
            }

            var vm = new ProfileFavoriteMovieCreateEditVM();
            vm.ProfileFavoriteMovie = profileFavoriteMovie;
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title),
                vm.ProfileFavoriteMovie.MovieDetailsId);
            vm.UserProfileSelectList = new SelectList(
                await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
                nameof(UserProfile.Id), nameof(UserProfile.Name),
                vm.ProfileFavoriteMovie.UserProfileId);
            return View(vm);
        }

        // POST: ProfileFavoriteMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ProfileFavoriteMovie profileFavoriteMovie)
        {
            if (id != profileFavoriteMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(profileFavoriteMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileFavoriteMovieExists(profileFavoriteMovie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var vm = new ProfileFavoriteMovieCreateEditVM();
            vm.ProfileFavoriteMovie = profileFavoriteMovie;
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title),
                vm.ProfileFavoriteMovie.MovieDetailsId);
            vm.UserProfileSelectList = new SelectList(
                await _context.UserProfiles.Select(p => new {p.Id, p.Name}).ToListAsync(),
                nameof(UserProfile.Id), nameof(UserProfile.Name),
                vm.ProfileFavoriteMovie.UserProfileId);
            return View(vm);
        }

        // GET: ProfileFavoriteMovies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var profileFavoriteMovie = await _context.ProfileFavoriteMovies
                .Include(p => p.MovieDetails)
                .Include(p => p.UserProfile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (profileFavoriteMovie == null)
            {
                return NotFound();
            }

            return View(profileFavoriteMovie);
        }

        // POST: ProfileFavoriteMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var profileFavoriteMovie = await _context.ProfileFavoriteMovies.FindAsync(id);
            _context.ProfileFavoriteMovies.Remove(profileFavoriteMovie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProfileFavoriteMovieExists(Guid id)
        {
            return _context.ProfileFavoriteMovies.Any(e => e.Id == id);
        }
    }
}
