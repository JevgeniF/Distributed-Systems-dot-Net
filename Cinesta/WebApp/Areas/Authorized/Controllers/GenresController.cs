#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator")]
public class GenresController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<GenresController> _logger;

    public GenresController(AppDbContext context, ILogger<GenresController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/Genres
    public async Task<IActionResult> Index()
    {
        return View(await _context.Genres.ToListAsync());
    }

    // GET: Authorized/Genres/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _context.Genres.FirstOrDefaultAsync(m => m.Id == id);
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
            _context.Add(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(genre);
    }

    // GET: Authorized/Genres/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var genre = await _context.Genres.FindAsync(id);
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
            var genreFromDb = await _context.Genres.AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == genre.Id);
            if (genreFromDb == null) return NotFound();

            try
            {
                genreFromDb.Naming.SetTranslation(genre.Naming);
                genre.Naming = genreFromDb.Naming;

                _context.Update(genre);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(genre.Id))
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

        var genre = await _context.Genres
            .FirstOrDefaultAsync(m => m.Id == id);
        if (genre == null) return NotFound();

        return View(genre);
    }


    // POST: Authorized/Genres/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var genre = await _context.Genres.FindAsync(id);
        _context.Genres.Remove(genre!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool GenreExists(Guid id)
    {
        return _context.Genres.Any(e => e.Id == id);
    }
}