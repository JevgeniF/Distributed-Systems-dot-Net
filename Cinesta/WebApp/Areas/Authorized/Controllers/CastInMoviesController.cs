#nullable disable
#pragma warning disable CS1591
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
public class CastInMoviesController : Controller
{
    private readonly IAppBll _bll;

    public CastInMoviesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/CastInMovies
    public async Task<IActionResult> Index()
    {
        return View(await _bll.CastInMovie.IncludeGetAllAsync());
    }

    // GET: Admin/CastInMovies/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var castInMovie = await _bll.CastInMovie.IncludeFirstOrDefaultAsync(id.Value);
        if (castInMovie == null) return NotFound();

        return View(castInMovie);
    }

    // GET: CastInMovies/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CastInMovieCreateEditVm
        {
            CastRoleSelectList = new SelectList((await _bll.CastRole.GetAllAsync())
                .Select(c => new { c.Id, c.Naming }),
                nameof(CastRole.Id), nameof(CastRole.Naming)),
            PersonSelectList = new SelectList((await _bll.Person.GetAllAsync())
                .Select(p => new { p.Id, FullName = string.Join(" ", p.Name, p.Surname) }),
                nameof(Person.Id), "FullName"),
            MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
                .Select(m => new { m.Id, m.Title }),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title))
        };

        return View(vm);
    }

    // POST: CastInMovies/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CastInMovieCreateEditVm vm)
    {
        if (ModelState.IsValid)
        {
            _bll.CastInMovie.Add(vm.CastInMovie);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.CastRoleSelectList = new SelectList(
            (await _bll.CastRole.GetAllAsync()).Select(c => new { c.Id, c.Naming }),
            nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie.CastRoleId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _bll.MovieDetails.GetAllAsync()).Select(m => new { m.Id, m.Title }),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
        vm.PersonSelectList = new SelectList(
            (await _bll.Person.GetAllAsync())
            .Select(p => new { p.Id, FullName = string.Join(" ", p.Name, p.Surname) }), nameof(Person.Id),
            "FullName", vm.CastInMovie.PersonId);
        return View(vm);
    }

    // GET: CastInMovies/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var castInMovie = await _bll.CastInMovie.FirstOrDefaultAsync(id.Value);
        if (castInMovie == null) return NotFound();
        var vm = new CastInMovieCreateEditVm
        {
            CastInMovie = castInMovie
        };
        vm.CastRoleSelectList = new SelectList(
            (await _bll.CastRole.GetAllAsync()).Select(c => new { c.Id, c.Naming }),
            nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie.CastRoleId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _bll.MovieDetails.GetAllAsync()).Select(m => new { m.Id, m.Title }),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
        vm.PersonSelectList = new SelectList(
            (await _bll.Person.GetAllAsync())
            .Select(p => new { p.Id, FullName = string.Join(" ", p.Name, p.Surname) }), nameof(Person.Id),
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
                _bll.CastInMovie.Update(castInMovie);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CastInMovieExists(castInMovie.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new CastInMovieCreateEditVm
        {
            CastInMovie = castInMovie
        };
        vm.CastRoleSelectList = new SelectList(
            (await _bll.CastRole.GetAllAsync()).Select(c => new { c.Id, c.Naming }),
            nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie.CastRoleId);
        vm.MovieDetailsSelectList = new SelectList(
            (await _bll.MovieDetails.GetAllAsync()).Select(m => new { m.Id, m.Title }),
            nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
        vm.PersonSelectList = new SelectList(
            (await _bll.Person.GetAllAsync())
            .Select(p => new { p.Id, FullName = string.Join(" ", p.Name, p.Surname) }), nameof(Person.Id),
            "FullName", vm.CastInMovie.PersonId);
        return View(vm);
    }


    // GET: Admin/CastInMovies/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var castInMovie = await _bll.CastInMovie.IncludeFirstOrDefaultAsync(id.Value);
        if (castInMovie == null) return NotFound();

        return View(castInMovie);
    }

    // POST: Admin/CastInMovies/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.CastInMovie.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CastInMovieExists(Guid id)
    {
        return await _bll.CastInMovie.ExistsAsync(id);
    }
}