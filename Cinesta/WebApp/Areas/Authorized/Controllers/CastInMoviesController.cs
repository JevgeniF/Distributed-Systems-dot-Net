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
public class CastInMoviesController : Controller
{
    private readonly ILogger<CastInMoviesController> _logger;
    private readonly IAppPublic _public;

    public CastInMoviesController(IAppPublic appPublic, ILogger<CastInMoviesController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/CastInMovies
    public async Task<IActionResult> Index()
    {
        return View(await _public.CastInMovie.IncludeGetAllAsync());
    }

    // GET: Authorized/CastInMovies/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var castInMovie = await _public.CastInMovie.IncludeFirstOrDefaultAsync(id.Value);
        if (castInMovie == null) return NotFound();

        return View(castInMovie);
    }

    // GET: CastInMovies/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CastInMovieCreateEditVM
        {
            CastRoleSelectList = new SelectList(
                (await _public.CastRole.GetAllAsync()).Select(c => new {c.Id, c.Naming}),
                nameof(CastRole.Id), nameof(CastRole.Naming)),
            MovieDetailsSelectList = new SelectList(
                (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title)),
            PersonSelectList = new SelectList(
                (await _public.Person.GetAllAsync())
                .Select(p => new {p.Id, FullName = string.Join(" ", p.Name, p.Surname)}),
                nameof(Person.Id),
                "FullName")
        };
        return View(vm);
    }

    // POST: CastInMovies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    // TODO: NAME + SURNAME in Person Select List
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CastInMovieCreateEditVM vm)
    {
        if (ModelState.IsValid)
        {
            _public.CastInMovie.Add(vm.CastInMovie);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.CastRoleSelectList = new SelectList(
            (await _public.CastRole.GetAllAsync()).Select(c => new {c.Id, c.Naming}),
            nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie!.CastRoleId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
        vm.PersonSelectList = new SelectList(
            (await _public.Person.GetAllAsync())
            .Select(p => new {p.Id, FullName = string.Join(" ", p.Name, p.Surname)}),
            nameof(Person.Id),
            "FullName", vm.CastInMovie.PersonId);
        return View(vm);
    }

    // GET: CastInMovies/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var castInMovie = await _public.CastInMovie.FirstOrDefaultAsync(id.Value);
        if (castInMovie == null) return NotFound();
        var vm = new CastInMovieCreateEditVM
        {
            CastInMovie = castInMovie
        };
        vm.CastRoleSelectList = new SelectList(
            (await _public.CastRole.GetAllAsync()).Select(c => new {c.Id, c.Naming}),
            nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie!.CastRoleId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
        vm.PersonSelectList = new SelectList(
            (await _public.Person.GetAllAsync())
            .Select(p => new {p.Id, FullName = string.Join(" ", p.Name, p.Surname)}),
            nameof(Person.Id),
            "FullName", vm.CastInMovie.PersonId);
        return View(vm);
    }

    // POST: CastInMovies/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CastInMovie castInMovie)
    {
        if (id != castInMovie.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _public.CastInMovie.Update(castInMovie);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CastInMovieExists(castInMovie.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new CastInMovieCreateEditVM
        {
            CastInMovie = castInMovie
        };
        vm.CastRoleSelectList = new SelectList(
            (await _public.CastRole.GetAllAsync()).Select(c => new {c.Id, c.Naming}),
            nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie!.CastRoleId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _public.MovieDetails.GetAllAsync()).Select(m => new {m.Id, m.Title}),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
        vm.PersonSelectList = new SelectList(
            (await _public.Person.GetAllAsync())
            .Select(p => new {p.Id, FullName = string.Join(" ", p.Name, p.Surname)}),
            nameof(Person.Id),
            "FullName", vm.CastInMovie.PersonId);
        return View(vm);
    }


    // GET: Authorized/CastInMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var castInMovie = await _public.CastInMovie.IncludeFirstOrDefaultAsync(id.Value);
        if (castInMovie == null) return NotFound();

        return View(castInMovie);
    }

    // POST: Authorized/CastInMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.CastInMovie.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CastInMovieExists(Guid id)
    {
        return await _public.CastInMovie.ExistsAsync(id);
    }
}