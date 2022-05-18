#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class UserRatingsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRatingsController> _logger;

    public UserRatingsController(AppDbContext context, ILogger<UserRatingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/UserRatings
    public async Task<IActionResult> Index()
    {
        return View(await _context.UserRatings.Include(u => u.AppUser)
            .Include(u => u.MovieDetails).ToListAsync());
    }

    // GET: Authorized/UserRatings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _context.UserRatings
            .Include(u => u.AppUser)
            .Include(u => u.MovieDetails)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (userRating == null) return NotFound();

        return View(userRating);
    }


    // GET: Authorized/UserRatings/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserRatingCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _context.MovieDetails.ToListAsync())
                .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
        };
        return View(vm);
    }

    // POST: UserRatings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserRatingCreateEditVM vm)
    {
        vm.UserRating.AppUserId = User.GetUserId();
        if (ModelState.IsValid)
        {
            _context.UserRatings.Add(vm.UserRating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList((await _context.MovieDetails.ToListAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }

    // GET: UserRatings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();
        var userRating = await _context.UserRatings.FirstOrDefaultAsync(u => u.Id == id);
        if (userRating == null) return NotFound();

        var vm = new UserRatingCreateEditVM
        {
            UserRating = userRating
        };
        vm.MovieDetailsSelectList = new SelectList((await _context.MovieDetails.ToListAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }

    // POST: UserRatings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserRating userRating)
    {
        if (id != userRating.Id) return NotFound();
        userRating.AppUserId = User.GetUserId();

        if (ModelState.IsValid)
        {
            var userRatingsFromDb = await _context.UserRatings.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (userRatingsFromDb == null) return NotFound();

            try
            {
                userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
                userRating.Comment = userRatingsFromDb.Comment;

                _context.UserRatings.Update(userRating);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRatingExists(userRating.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new UserRatingCreateEditVM
        {
            UserRating = userRating
        };
        vm.MovieDetailsSelectList = new SelectList((await _context.MovieDetails.ToListAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }


    // GET: Admin/UserRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _context.UserRatings
            .Include(u => u.AppUser)
            .Include(u => u.MovieDetails)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (userRating == null) return NotFound();

        return View(userRating);
    }

    // POST: Admin/UserRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userRating = await _context.UserRatings.FindAsync(id);
        _context.UserRatings.Remove(userRating!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserRatingExists(Guid id)
    {
        return _context.UserRatings.Any(e => e.Id == id);
    }
}