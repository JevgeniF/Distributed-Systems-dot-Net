#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator")]
public class MovieTypesController : Controller
{
    private readonly ILogger<MovieTypesController> _logger;
    private readonly IAppPublic _public;

    public MovieTypesController(IAppPublic appPublic, ILogger<MovieTypesController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/MovieTypes
    public async Task<IActionResult> Index()
    {
        return View(await _public.MovieType.GetAllAsync());
    }

    // GET: Authorized/MovieTypes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _public.MovieType.FirstOrDefaultAsync(id.Value);
        if (movieType == null) return NotFound();

        return View(movieType);
    }

    // GET: Authorized/MovieTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authorized/MovieTypes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        MovieType movieType)
    {
        if (ModelState.IsValid)
        {
            movieType.Id = Guid.NewGuid();
            _public.MovieType.Add(movieType);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(movieType);
    }

    // GET: Authorized/MovieTypes/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _public.MovieType.FirstOrDefaultAsync(id.Value);
        if (movieType == null) return NotFound();
        return View(movieType);
    }

    // POST: Authorized/MovieTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        MovieType movieType)
    {
        if (id != movieType.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var movieTypeFromDb = await _public.MovieType.FirstOrDefaultAsync(id);
            if (movieTypeFromDb == null) return NotFound();

            try
            {
                movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
                movieType.Naming = movieTypeFromDb.Naming;

                _public.MovieType.Update(movieType);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MovieTypeExists(movieType.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(movieType);
    }

    // GET: Authorized/MovieTypes/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _public.MovieType.FirstOrDefaultAsync(id.Value);
        if (movieType == null) return NotFound();

        return View(movieType);
    }

    // POST: Authorized/MovieTypes/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.MovieType.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieTypeExists(Guid id)
    {
        return await _public.MovieType.ExistsAsync(id);
    }
}