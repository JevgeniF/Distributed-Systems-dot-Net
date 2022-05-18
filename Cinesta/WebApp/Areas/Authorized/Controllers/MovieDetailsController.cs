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
public class MovieDetailsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<MovieDetailsController> _logger;

    public MovieDetailsController(AppDbContext context, ILogger<MovieDetailsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/MovieDetails
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.MovieDetails.Include(m => m.AgeRating).Include(m => m.MovieType);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Authorized/MovieDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movieDetails = await _context.MovieDetails
            .Include(m => m.AgeRating)
            .Include(m => m.MovieType)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movieDetails == null)
        {
            return NotFound();
        }

        return View(movieDetails);
    }

    // GET: Authorized/MovieDetails/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieDetailsCreateEditVM
        {
            AgeRatingSelectList = new SelectList(
                await _context.AgeRatings.Select(r => new {r.Id, r.Naming}).ToListAsync(),
                nameof(AgeRating.Id), nameof(AgeRating.Naming)),
            MovieTypeSelectList = new SelectList(
                await _context.MovieTypes.Select(t => new {t.Id, t.Naming}).ToListAsync(),
                nameof(MovieType.Id), nameof(MovieType.Naming))
        };
        return View(vm);
    }

    // POST: MovieDetails/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieDetailsCreateEditVM vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.MovieDetails!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        vm.AgeRatingSelectList = new SelectList(
            await _context.AgeRatings.Select(r => new {r.Id, r.Naming}).ToListAsync(),
            nameof(AgeRating.Id), nameof(AgeRating.Naming), vm.MovieDetails!.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList(
            await _context.MovieTypes.Select(t => new {t.Id, t.Naming}).ToListAsync(),
            nameof(MovieType.Id), nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }


    // GET: MovieDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movieDetails = await _context.MovieDetails.FindAsync(id);
        if (movieDetails == null)
        {
            return NotFound();
        }

        var vm = new MovieDetailsCreateEditVM
        {
            MovieDetails = movieDetails
        };
        vm.AgeRatingSelectList = new SelectList(
            await _context.AgeRatings.Select(r => new {r.Id, r.Naming}).ToListAsync(),
            nameof(AgeRating.Id), nameof(AgeRating.Naming), vm.MovieDetails.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList(
            await _context.MovieTypes.Select(t => new {t.Id, t.Naming}).ToListAsync(),
            nameof(MovieType.Id), nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }



    // POST: MovieDetails/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MovieDetails movieDetails)
        {
            if (id != movieDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var movieDetailsFromDb = await _context.MovieDetails.AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == movieDetails.Id);
                if (movieDetailsFromDb == null)
                {
                    return NotFound();
                }
                try
                {
                    movieDetailsFromDb.Title.SetTranslation(movieDetails.Title);
                    movieDetails.Title = movieDetailsFromDb.Title;
                    
                    movieDetailsFromDb.Description.SetTranslation(movieDetails.Description);
                    movieDetails.Description = movieDetailsFromDb.Description;
                    
                    _context.Update(movieDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieDetailsExists(movieDetails.Id))
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
            var vm = new MovieDetailsCreateEditVM
            {
                MovieDetails = movieDetails
            };
            vm.AgeRatingSelectList = new SelectList(
                await _context.AgeRatings.Select(r => new {r.Id, r.Naming}).ToListAsync(),
                nameof(AgeRating.Id), nameof(AgeRating.Naming), vm.MovieDetails.AgeRatingId);
            vm.MovieTypeSelectList = new SelectList(
                await _context.MovieTypes.Select(t => new {t.Id, t.Naming}).ToListAsync(),
                nameof(MovieType.Id), nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
            return View(vm);
        }

    // GET: Authorized/MovieDetails/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movieDetails = await _context.MovieDetails
            .Include(m => m.AgeRating)
            .Include(m => m.MovieType)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movieDetails == null)
        {
            return NotFound();
        }

        return View(movieDetails);
    }

    // POST: Authorized/MovieDetails/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var movieDetails = await _context.MovieDetails.FindAsync(id);
        _context.MovieDetails.Remove(movieDetails!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieDetailsExists(Guid id)
    {
        return _context.MovieDetails.Any(e => e.Id == id);
    }

}