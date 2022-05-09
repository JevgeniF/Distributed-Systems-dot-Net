#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin")]
public class MovieTypesController : Controller
{
    private readonly IAppUOW _uow;

    public MovieTypesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/MovieTypes
    public async Task<IActionResult> Index()
    {
        return View(await _uow.MovieType.GetAllAsync());
    }

    // GET: Admin/MovieTypes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _uow.MovieType
            .FirstOrDefaultAsync(id.Value);
        if (movieType == null) return NotFound();

        return View(movieType);
    }

    // GET: Admin/MovieTypes/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/MovieTypes/Create
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
            _uow.MovieType.Add(movieType);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(movieType);
    }

    // GET: Admin/MovieTypes/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _uow.MovieType.FirstOrDefaultAsync(id.Value);
        if (movieType == null) return NotFound();
        return View(movieType);
    }

    // POST: Admin/MovieTypes/Edit/5
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
            var movieTypeFromDb = await _uow.MovieType.FirstOrDefaultAsync(id);
            if (movieTypeFromDb == null) return NotFound();

            try
            {
                movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
                movieType.Naming = movieTypeFromDb.Naming;

                _uow.MovieType.Update(movieType);
                await _uow.SaveChangesAsync();
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

    // GET: Admin/MovieTypes/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _uow.MovieType
            .FirstOrDefaultAsync(id.Value);
        if (movieType == null) return NotFound();

        return View(movieType);
    }

    // POST: Admin/MovieTypes/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.MovieType.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> MovieTypeExists(Guid id)
    {
        return await _uow.MovieType.ExistsAsync(id);
    }
}