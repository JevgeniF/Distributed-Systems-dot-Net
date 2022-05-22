#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
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
    private readonly ILogger<MovieGenresController> _logger;
    private readonly IAppPublic _public;

    public MovieGenresController(IAppPublic appPublic, ILogger<MovieGenresController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/MovieGenres
    public async Task<IActionResult> Index()
    {
        return View(await _public.MovieGenre.IncludeGetAllAsync());
    }

    // GET: Authorized/MovieGenres/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieGenre = await _public.MovieGenre.IncludeFirstOrDefaultAsync(id.Value);
        if (movieGenre == null) return NotFound();

        return View(movieGenre);
    }

    // GET: Authorized/MovieGenres/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieGenreCreateEditVM
        {
            GenreSelectList = new SelectList(
                (await _public.Genre.GetAllAsync()).Select(g => new {g.Id, g.Naming}),
                nameof(Genre.Id), nameof(Genre.Naming)),
            MovieDetailsSelectList = new SelectList(
                (await _public.MovieDetails.GetAllAsync()).Select(d => new {d.Id, d.Title}),
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
            _public.MovieGenre.Add(vm.MovieGenre!);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.GenreSelectList = new SelectList(
            (await _public.Genre.GetAllAsync()).Select(g => new {g.Id, g.Naming}),
            nameof(Genre.Id), nameof(Genre.Naming), vm.MovieGenre!.GenreId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(d => new {d.Id, d.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
        return View(vm);
    }

    // GET: MovieGenres/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieGenre = await _public.MovieGenre.FirstOrDefaultAsync(id.Value);
        if (movieGenre == null) return NotFound();
        var vm = new MovieGenreCreateEditVM
        {
            MovieGenre = movieGenre
        };
        vm.GenreSelectList = new SelectList(
            (await _public.Genre.GetAllAsync()).Select(g => new {g.Id, g.Naming}),
            nameof(Genre.Id), nameof(Genre.Naming), vm.MovieGenre!.GenreId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(d => new {d.Id, d.Title}),
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
        if (id != movieGenre.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _public.MovieGenre.Update(movieGenre);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieGenreExists(movieGenre.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new MovieGenreCreateEditVM
        {
            MovieGenre = movieGenre
        };
        vm.GenreSelectList = new SelectList(
            (await _public.Genre.GetAllAsync()).Select(g => new {g.Id, g.Naming}),
            nameof(Genre.Id), nameof(Genre.Naming), vm.MovieGenre!.GenreId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(d => new {d.Id, d.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
        return View(vm);
    }


    // GET: Authorized/MovieGenres/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieGenre = await _public.MovieGenre.IncludeFirstOrDefaultAsync(id.Value);
        if (movieGenre == null) return NotFound();

        return View(movieGenre);
    }

    // POST: Authorized/MovieGenres/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.MovieGenre.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieGenreExists(Guid id)
    {
        return await _public.MovieGenre.ExistsAsync(id);
    }
}