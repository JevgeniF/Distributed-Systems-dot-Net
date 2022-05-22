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
public class AgeRatingsController : Controller
{
    private readonly ILogger<AgeRatingsController> _logger;
    private readonly IAppPublic _public;

    public AgeRatingsController(IAppPublic appPublic, ILogger<AgeRatingsController> logger)
    {
        _logger = logger;
        _public = appPublic;
    }

    // GET: Authorized/AgeRatings
    public async Task<IActionResult> Index()
    {
        return View(await _public.AgeRating.GetAllAsync());
    }

    // GET: Authorized/AgeRatings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _public.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();

        return View(ageRating);
    }

    // GET: Authorized/AgeRatings/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authorized/AgeRatings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        AgeRating ageRating)
    {
        if (ModelState.IsValid)
        {
            ageRating.Id = Guid.NewGuid();
            _public.AgeRating.Add(ageRating);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(ageRating);
    }

    // GET: Authorized/AgeRatings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _public.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();
        return View(ageRating);
    }

    // POST: Authorized/AgeRatings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        AgeRating ageRating)
    {
        if (id != ageRating.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _public.AgeRating.Update(ageRating);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AgeRatingExists(ageRating.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(ageRating);
    }

    // GET: Authorized/AgeRatings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _public.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();

        return View(ageRating);
    }

    // POST: Authorized/AgeRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.AgeRating.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> AgeRatingExists(Guid id)
    {
        return await _public.AgeRating.ExistsAsync(id);
    }
}