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
public class MovieDbScoresController : Controller
{
    private readonly IAppBll _bll;

    public MovieDbScoresController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/MovieDbScores
    public async Task<IActionResult> Index()
    {
        return View(await _bll.MovieDbScore.IncludeGetAllAsync());
    }

    // GET: Admin/MovieDbScores/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDbScore = await _bll.MovieDbScore.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDbScore == null) return NotFound();

        return View(movieDbScore);
    }

    // GET: MovieDbScores/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieDbScoreCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
                .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
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
            _bll.MovieDbScore.Add(vm.MovieDbScore);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.MovieDbScore.MovieDetailsId);
        return View(vm);
    }

    // GET: MovieDbScores/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDbScore = await _bll.MovieDbScore.FirstOrDefaultAsync(id.Value);
        if (movieDbScore == null) return NotFound();

        var vm = new MovieDbScoreCreateEditVM
        {
            MovieDbScore = movieDbScore
        };
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.MovieDbScore.MovieDetailsId);
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
                _bll.MovieDbScore.Update(movieDbScore);
                await _bll.SaveChangesAsync();
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
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.MovieDbScore.MovieDetailsId);
        return View(vm);
    }


    // GET: Admin/MovieDbScores/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDbScore = await _bll.MovieDbScore.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDbScore == null) return NotFound();

        return View(movieDbScore);
    }

    // POST: Admin/MovieDbScores/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.MovieDbScore.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieDbScoreExists(Guid id)
    {
        return await _bll.MovieDbScore.ExistsAsync(id);
    }
}