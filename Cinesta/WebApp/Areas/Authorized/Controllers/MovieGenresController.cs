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
public class MovieGenresController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<MovieGenresController> _logger;

    public MovieGenresController(AppDbContext context, ILogger<MovieGenresController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/MovieGenres
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.MovieGenres.Include(m => m.Genre).Include(m => m.MovieDetails);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Authorized/MovieGenres/Details/5
    public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieGenre = await _context.MovieGenres
                .Include(m => m.Genre)
                .Include(m => m.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieGenre == null)
            {
                return NotFound();
            }

            return View(movieGenre);
        }

        // GET: Authorized/MovieGenres/Create
        public async Task<IActionResult> Create()
        {
            var vm = new MovieGenreCreateEditVM
            {
                GenreSelectList = new SelectList(
                    await _context.Genres.Select(g => new { g.Id, g.Naming}).ToListAsync(),
                    nameof(Genre.Id), nameof(Genre.Naming)),
                MovieDetailsSelectList = new SelectList(
                    _context.MovieDetails.Select(d => new {d.Id, d.Title}),
                    nameof(MovieDetails.Id), nameof(MovieDetails.Title))
            };
            return View(vm);
        }

        // POST: MovieGenres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieGenreCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.MovieGenre!);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.GenreSelectList = new SelectList(
                await _context.Genres.Select(g => new { g.Id, g.Naming}).ToListAsync(),
                nameof(Genre.Id), nameof(Genre.Naming), vm.MovieGenre!.GenreId);
            vm.MovieDetailsSelectList = new SelectList(
                _context.MovieDetails.Select(d => new {d.Id, d.Title}),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
            return View(vm);
        }

        // GET: MovieGenres/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieGenre = await _context.MovieGenres.FindAsync(id);
            if (movieGenre == null)
            {
                return NotFound();
            }
            var vm = new MovieGenreCreateEditVM
            {
                MovieGenre = movieGenre
            };
            vm.GenreSelectList = new SelectList(
                await _context.Genres.Select(g => new { g.Id, g.Naming}).ToListAsync(),
                nameof(Genre.Id), nameof(Genre.Naming), vm.MovieGenre.GenreId);
            vm.MovieDetailsSelectList = new SelectList(
                _context.MovieDetails.Select(d => new {d.Id, d.Title}),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
            return View(vm);
        }

        // POST: MovieGenres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, MovieGenre movieGenre)
        {
            if (id != movieGenre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieGenre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieGenreExists(movieGenre.Id))
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
            var vm = new MovieGenreCreateEditVM
            {
                MovieGenre = movieGenre
            };
            vm.GenreSelectList = new SelectList(
                await _context.Genres.Select(g => new { g.Id, g.Naming}).ToListAsync(),
                nameof(Genre.Id), nameof(Genre.Naming), vm.MovieGenre.GenreId);
            vm.MovieDetailsSelectList = new SelectList(
                _context.MovieDetails.Select(d => new {d.Id, d.Title}),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
            return View(vm);
        }




        // GET: Authorized/MovieGenres/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieGenre = await _context.MovieGenres
                .Include(m => m.Genre)
                .Include(m => m.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieGenre == null)
            {
                return NotFound();
            }

            return View(movieGenre);
        }

        // POST: Authorized/MovieGenres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movieGenre = await _context.MovieGenres.FindAsync(id);
            _context.MovieGenres.Remove(movieGenre!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieGenreExists(Guid id)
        {
            return _context.MovieGenres.Any(e => e.Id == id);
        }
    }
