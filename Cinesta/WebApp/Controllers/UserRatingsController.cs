#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Identity;
using App.Domain.Movie;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class UserRatingsController : Controller
    {
        private readonly AppDbContext _context;

        public UserRatingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserRatings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserRatings.Include(u => u.AppUser).Include(u => u.MovieDetails);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserRatings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRating = await _context.UserRatings
                .Include(u => u.AppUser)
                .Include(u => u.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRating == null)
            {
                return NotFound();
            }

            return View(userRating);
        }

        // GET: UserRatings/Create
        public async Task<IActionResult> Create()
        {
            var vm = new UserRatingCreateEditVM();
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id));
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title));
            return View(vm);
        }

        // POST: UserRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserRatingCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.UserRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.UserRating.AppUserId);
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
            return View(vm);
        }

        // GET: UserRatings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRating = await _context.UserRatings.FindAsync(id);
            if (userRating == null)
            {
                return NotFound();
            }

            var vm = new UserRatingCreateEditVM();
            vm.UserRating = userRating;
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.UserRating.AppUserId);
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
            return View(vm);
        }

        // POST: UserRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserRating userRating)
        {
            if (id != userRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRatingExists(userRating.Id))
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
            var vm = new UserRatingCreateEditVM();
            vm.UserRating = userRating;
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.UserRating.AppUserId);
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
            return View(vm);
        }

        // GET: UserRatings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRating = await _context.UserRatings
                .Include(u => u.AppUser)
                .Include(u => u.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userRating == null)
            {
                return NotFound();
            }

            return View(userRating);
        }

        // POST: UserRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userRating = await _context.UserRatings.FindAsync(id);
            _context.UserRatings.Remove(userRating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRatingExists(Guid id)
        {
            return _context.UserRatings.Any(e => e.Id == id);
        }
    }
}
