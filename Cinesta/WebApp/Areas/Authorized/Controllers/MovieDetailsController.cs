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
public class MovieDetailsController : Controller
{
    private readonly ILogger<MovieDetailsController> _logger;
    private readonly IAppPublic _public;

    public MovieDetailsController(IAppPublic appPublic, ILogger<MovieDetailsController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/MovieDetails
    public async Task<IActionResult> Index()
    {
        return View(await _public.MovieDetails.IncludeGetAllAsync());
    }

    // GET: Authorized/MovieDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDetails = await _public.MovieDetails.FirstOrDefaultAsync(id.Value);
        if (movieDetails == null) return NotFound();

        return View(movieDetails);
    }

    // GET: Authorized/MovieDetails/Create
    public async Task<IActionResult> Create()
    {
        var vm = new MovieDetailsCreateEditVM
        {
            AgeRatingSelectList = new SelectList(
                (await _public.AgeRating.GetAllAsync()).Select(r => new {r.Id, r.Naming}),
                nameof(AgeRating.Id), nameof(AgeRating.Naming)),
            MovieTypeSelectList = new SelectList(
                (await _public.MovieType.GetAllAsync()).Select(t => new {t.Id, t.Naming}),
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
            _public.MovieDetails.Add(vm.MovieDetails!);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.AgeRatingSelectList = new SelectList(
            (await _public.AgeRating.GetAllAsync()).Select(r => new {r.Id, r.Naming}),
            nameof(AgeRating.Id), nameof(AgeRating.Naming), vm.MovieDetails!.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList(
            (await _public.MovieType.GetAllAsync()).Select(t => new {t.Id, t.Naming}),
            nameof(MovieType.Id), nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }


    // GET: MovieDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDetails = await _public.MovieDetails.FirstOrDefaultAsync(id.Value);
        if (movieDetails == null) return NotFound();

        var vm = new MovieDetailsCreateEditVM
        {
            MovieDetails = movieDetails
        };
        vm.AgeRatingSelectList = new SelectList(
            (await _public.AgeRating.GetAllAsync()).Select(r => new {r.Id, r.Naming}),
            nameof(AgeRating.Id), nameof(AgeRating.Naming), vm.MovieDetails!.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList(
            (await _public.MovieType.GetAllAsync()).Select(t => new {t.Id, t.Naming}),
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
        if (id != movieDetails.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var movieDetailsFromDb = await _public.MovieDetails.FirstOrDefaultAsync(id);
            if (movieDetailsFromDb == null) return NotFound();
            try
            {
                movieDetailsFromDb.Title.SetTranslation(movieDetails.Title);
                movieDetails.Title = movieDetailsFromDb.Title;

                movieDetailsFromDb.Description.SetTranslation(movieDetails.Description);
                movieDetails.Description = movieDetailsFromDb.Description;

                _public.MovieDetails.Update(movieDetails);
                await _public.SaveChangesAsync();
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
        vm.AgeRatingSelectList = new SelectList(
            (await _public.AgeRating.GetAllAsync()).Select(r => new {r.Id, r.Naming}),
            nameof(AgeRating.Id), nameof(AgeRating.Naming), vm.MovieDetails!.AgeRatingId);
        vm.MovieTypeSelectList = new SelectList(
            (await _public.MovieType.GetAllAsync()).Select(t => new {t.Id, t.Naming}),
            nameof(MovieType.Id), nameof(MovieType.Naming), vm.MovieDetails.MovieTypeId);
        return View(vm);
    }

    // GET: Authorized/MovieDetails/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieDetails = await _public.MovieDetails.IncludeFirstOrDefaultAsync(id.Value);
        if (movieDetails == null) return NotFound();

        return View(movieDetails);
    }

    // POST: Authorized/MovieDetails/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.MovieDetails.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieDetailsExists(Guid id)
    {
        return await _public.MovieDetails.ExistsAsync(id);
    }
}