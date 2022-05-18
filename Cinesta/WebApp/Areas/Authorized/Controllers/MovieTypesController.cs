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
public class MovieTypesController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<MovieTypesController> _logger;

    public MovieTypesController(AppDbContext context, ILogger<MovieTypesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/MovieTypes
    public async Task<IActionResult> Index()
    {
        return View(await _context.MovieTypes.ToListAsync());
    }

    // GET: Authorized/MovieTypes/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _context.MovieTypes
            .FirstOrDefaultAsync(m => m.Id == id);
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
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MovieType movieType)
    {
        if (ModelState.IsValid)
        {
            movieType.Id = Guid.NewGuid();
            _context.Add(movieType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(movieType);
    }

    // GET: Authorized/MovieTypes/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var movieType = await _context.MovieTypes.FindAsync(id);
        if (movieType == null) return NotFound();
        return View(movieType);
    }

    // POST: Authorized/MovieTypes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MovieType movieType)
    {
        if (id != movieType.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var movieTypeFromDb = await _context.MovieTypes.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == movieType.Id);
            if (movieTypeFromDb == null) return NotFound();

            try
            {
                movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
                movieType.Naming = movieTypeFromDb.Naming;

                _context.Update(movieType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieTypeExists(movieType.Id))
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

        var movieType = await _context.MovieTypes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movieType == null) return NotFound();

        return View(movieType);
    }

    // POST: Authorized/MovieTypes/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var movieType = await _context.MovieTypes.FindAsync(id);
        _context.MovieTypes.Remove(movieType!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieTypeExists(Guid id)
    {
        return _context.MovieTypes.Any(e => e.Id == id);
    }
}