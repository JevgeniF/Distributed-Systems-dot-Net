#nullable disable
#pragma warning disable CS1591
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator")]
public class GenresController : Controller
{
    private readonly IAppBll _bll;

    public GenresController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/Genres
    public async Task<IActionResult> Index()
    {
        return View(await _bll.Genre.GetAllAsync());
    }

    // GET: Admin/Genres/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _bll.Genre.FirstOrDefaultAsync(id.Value);
        if (genre == null) return NotFound();

        return View(genre);
    }

    // GET: Admin/Genres/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Genres/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Genre genre)
    {
        if (ModelState.IsValid)
        {
            genre.Id = Guid.NewGuid();
            _bll.Genre.Add(genre);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    // GET: Admin/Genres/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _bll.Genre.FirstOrDefaultAsync(id.Value);
        if (genre == null) return NotFound();
        return View(genre);
    }

    // POST: Admin/Genres/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        Genre genre)
    {
        if (id != genre.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var genreFromDb = await _bll.Genre.FirstOrDefaultAsync(id);
            if (genreFromDb == null) return NotFound();

            try
            {
                genreFromDb.Naming.SetTranslation(genre.Naming);
                genre.Naming = genreFromDb.Naming;

                _bll.Genre.Update(genre);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await GenreExists(genre.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    // GET: Admin/Genres/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _bll.Genre.FirstOrDefaultAsync(id.Value);
        if (genre == null) return NotFound();

        return View(genre);
    }

    // POST: Admin/Genres/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.Genre.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> GenreExists(Guid id)
    {
        return await _bll.Genre.ExistsAsync(id);
    }
}