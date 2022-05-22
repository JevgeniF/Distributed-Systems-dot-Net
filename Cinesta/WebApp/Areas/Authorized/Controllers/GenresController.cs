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
public class GenresController : Controller
{
    private readonly ILogger<GenresController> _logger;
    private readonly IAppPublic _public;

    public GenresController(IAppPublic appPublic, ILogger<GenresController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/Genres
    public async Task<IActionResult> Index()
    {
        return View(await _public.Genre.GetAllAsync());
    }

    // GET: Authorized/Genres/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _public.Genre.FirstOrDefaultAsync(id.Value);
        if (genre == null) return NotFound();

        return View(genre);
    }

    // GET: Authorized/Genres/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authorized/Genres/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Genre genre)
    {
        if (ModelState.IsValid)
        {
            genre.Id = Guid.NewGuid();
            _public.Genre.Add(genre);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    // GET: Authorized/Genres/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _public.Genre.FirstOrDefaultAsync(id.Value);
        if (genre == null) return NotFound();
        return View(genre);
    }

    // POST: Authorized/Genres/Edit/5
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
            var genreFromDb = await _public.Genre.FirstOrDefaultAsync(id);
            if (genreFromDb == null) return NotFound();

            try
            {
                genreFromDb.Naming.SetTranslation(genre.Naming);
                genre.Naming = genreFromDb.Naming;

                _public.Genre.Update(genre);
                await _public.SaveChangesAsync();
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


    // GET: Authorized/Genres/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _public.Genre.FirstOrDefaultAsync(id.Value);
        if (genre == null) return NotFound();

        return View(genre);
    }


    // POST: Authorized/Genres/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.Genre.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> GenreExists(Guid id)
    {
        return await _public.Genre.ExistsAsync(id);
    }
}