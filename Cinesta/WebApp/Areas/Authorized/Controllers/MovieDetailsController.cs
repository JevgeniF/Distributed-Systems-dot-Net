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
public class MovieDetailsController : Controller
{
    private readonly IAppBll _bll;

    public MovieDetailsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/MovieDetails
    public async Task<IActionResult> Index()
    {
        return View(await _bll.MovieDetails.IncludeGetAllAsync());
    }

    // GET: Admin/MovieDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDetails = await _bll.MovieDetails.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDetails == null) return NotFound();

        return View(movieDetails);
    }

    // GET: Admin/MovieDetails/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieDetailsCreateEditVM
        {
            AgeRatingSelectList = new SelectList((await _bll.AgeRating.GetAllAsync())
                .Select(r => new {r.Id, r.Naming}), nameof(AgeRating.Id),
                nameof(AgeRating.Naming)),
            MovieTypeSelectList = new SelectList((await _bll.MovieType.GetAllAsync())
                .Select(t => new {t.Id, t.Naming}), nameof(MovieType.Id),
                nameof(MovieType.Naming))
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
            _bll.MovieDetails.Add(vm.MovieDetails);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.AgeRatingSelectList = new SelectList((await _bll.AgeRating.GetAllAsync())
            .Select(r => new {r.Id, r.Naming}), nameof(AgeRating.Id),
            nameof(AgeRating.Naming), vm.MovieDetails.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList((await _bll.MovieType.GetAllAsync())
            .Select(t => new {t.Id, t.Naming}), nameof(MovieType.Id),
            nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }

    // GET: MovieDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDetails = await _bll.MovieDetails.FirstOrDefaultAsync(id.Value);
        if (movieDetails == null) return NotFound();

        var vm = new MovieDetailsCreateEditVM
        {
            MovieDetails = movieDetails
        };
        vm.AgeRatingSelectList = new SelectList((await _bll.AgeRating.GetAllAsync())
            .Select(r => new {r.Id, r.Naming}), nameof(AgeRating.Id),
            nameof(AgeRating.Naming), vm.MovieDetails.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList((await _bll.MovieType.GetAllAsync())
            .Select(t => new {t.Id, t.Naming}), nameof(MovieType.Id),
            nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }

    // POST: MovieDetails/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MovieDetails movieDetails)
    {
        if (id != movieDetails.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var movieDetailsFromDb = await _bll.MovieDetails.FirstOrDefaultAsync(id);
            if (movieDetailsFromDb == null) return NotFound();
            try
            {
                movieDetailsFromDb.Title.SetTranslation(movieDetails.Title);
                movieDetails.Title = movieDetailsFromDb.Title;

                movieDetailsFromDb.Description.SetTranslation(movieDetails.Description);
                movieDetails.Description = movieDetailsFromDb.Description;

                _bll.MovieDetails.Update(movieDetails);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieDetailsExists(movieDetails.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new MovieDetailsCreateEditVM
        {
            MovieDetails = movieDetails
        };
        vm.AgeRatingSelectList = new SelectList((await _bll.AgeRating.GetAllAsync())
            .Select(r => new {r.Id, r.Naming}), nameof(AgeRating.Id),
            nameof(AgeRating.Naming), vm.MovieDetails.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList((await _bll.MovieType.GetAllAsync())
            .Select(t => new {t.Id, t.Naming}), nameof(MovieType.Id),
            nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }


    // GET: Admin/MovieDetails/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDetails = await _bll.MovieDetails.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDetails == null) return NotFound();

        return View(movieDetails);
    }

    // POST: Admin/MovieDetails/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.MovieDetails.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieDetailsExists(Guid id)
    {
        return await _bll.MovieDetails.ExistsAsync(id);
    }
}