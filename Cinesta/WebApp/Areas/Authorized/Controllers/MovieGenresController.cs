#nullable disable
using App.BLL.DTO;
using App.Contracts.BLL;
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
    private readonly IAppBll _bll;

    public MovieGenresController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/MovieGenres
    public async Task<IActionResult> Index()
    {
        return View(await _bll.MovieGenre.IncludeGetAllAsync());
    }

    // GET: Admin/MovieGenres/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieGenre = await _bll.MovieGenre.IncludeFirstOrDefaultAsync(id.Value);
        if (movieGenre == null) return NotFound();

        return View(movieGenre);
    }

    // GET: Admin/MovieGenres/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieGenreCreateEditVM
        {
            GenreSelectList = new SelectList((await _bll.Genre.GetAllAsync())
                .Select(g => new {g.Id, g.Naming}), nameof(Genre.Id), nameof(Genre.Naming)),
            MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
                .Select(d => new {d.Id, d.Title}), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
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
            _bll.MovieGenre.Add(vm.MovieGenre);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.GenreSelectList = new SelectList((await _bll.Genre.GetAllAsync())
            .Select(g => new {g.Id, g.Naming}), nameof(Genre.Id), nameof(Genre.Naming),
            vm.MovieGenre.GenreId);
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(d => new {d.Id, d.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
        return View(vm);
    }

    // GET: MovieGenres/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieGenre = await _bll.MovieGenre.FirstOrDefaultAsync(id.Value);
        if (movieGenre == null) return NotFound();
        var vm = new MovieGenreCreateEditVM
        {
            MovieGenre = movieGenre
        };
        vm.GenreSelectList = new SelectList((await _bll.Genre.GetAllAsync())
            .Select(g => new {g.Id, g.Naming}), nameof(Genre.Id), nameof(Genre.Naming),
            vm.MovieGenre.GenreId);
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(d => new {d.Id, d.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
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
                _bll.MovieGenre.Update(movieGenre);
                await _bll.SaveChangesAsync();
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
        vm.GenreSelectList = new SelectList((await _bll.Genre.GetAllAsync())
            .Select(g => new {g.Id, g.Naming}), nameof(Genre.Id), nameof(Genre.Naming),
            vm.MovieGenre.GenreId);
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(d => new {d.Id, d.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.MovieGenre.MovieDetailsId);
        return View(vm);
    }


    // GET: Admin/MovieGenres/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieGenre = await _bll.MovieGenre.IncludeFirstOrDefaultAsync(id.Value);
        if (movieGenre == null) return NotFound();

        return View(movieGenre);
    }

    // POST: Admin/MovieGenres/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.MovieGenre.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieGenreExists(Guid id)
    {
        return await _bll.MovieGenre.ExistsAsync(id);
    }
}