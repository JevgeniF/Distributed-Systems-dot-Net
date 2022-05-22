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
public class MovieDbScoresController : Controller
{
    private readonly ILogger<MovieDbScoresController> _logger;
    private readonly IAppPublic _public;

    public MovieDbScoresController(IAppPublic appPublic, ILogger<MovieDbScoresController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/MovieDbScores
    public async Task<IActionResult> Index()
    {
        return View(await _public.MovieDbScore.IncludeGetAllAsync());
    }

    // GET: Authorized/MovieDbScores/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDbScore = await _public.MovieDbScore.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDbScore == null) return NotFound();

        return View(movieDbScore);
    }

    // GET: MovieDbScores/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieDbScoreCreateEditVM
        {
            MovieDetailsSelectList = new SelectList(
                (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title))
        };
        return View(vm);
    }

    // POST: MovieDbScores/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovieDbScoreCreateEditVM vm)
    {
        if (ModelState.IsValid)
        {
            _public.MovieDbScore.Add(vm.MovieDbScore!);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieDbScore!.MovieDetailsId);
        return View(vm);
    }


    // GET: MovieDbScores/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDbScore = await _public.MovieDbScore.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDbScore == null) return NotFound();

        var vm = new MovieDbScoreCreateEditVM
        {
            MovieDbScore = movieDbScore
        };
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieDbScore.MovieDetailsId);
        return View(vm);
    }


    // POST: MovieDbScores/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MovieDbScore movieDbScore)
    {
        if (id != movieDbScore.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _public.MovieDbScore.Update(movieDbScore);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieDbScoreExists(movieDbScore.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new MovieDbScoreCreateEditVM
        {
            MovieDbScore = movieDbScore
        };
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.MovieDbScore.MovieDetailsId);
        return View(vm);
    }


    // GET: Authorized/MovieDbScores/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDbScore = await _public.MovieDbScore.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDbScore == null) return NotFound();

        return View(movieDbScore);
    }

    // POST: Authorized/MovieDbScores/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.MovieDbScore.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieDbScoreExists(Guid id)
    {
        return await _public.MovieDbScore.ExistsAsync(id);
    }
}