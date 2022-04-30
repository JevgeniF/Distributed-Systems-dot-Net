#nullable disable
using App.Contracts.DAL;
using App.Domain.MovieStandardDetails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin")]
public class AgeRatingsController : Controller
{
    private readonly IAppUOW _uow;

    public AgeRatingsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/AgeRatings
    public async Task<IActionResult> Index()
    {
        var result = await _uow.AgeRating.GetAllAsync();
        return View(result);
    }

    // GET: Admin/AgeRatings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _uow.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();

        return View(ageRating);
    }

    // GET: Admin/AgeRatings/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/AgeRatings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] AgeRating ageRating)
    {
        if (ModelState.IsValid)
        {
            ageRating.Id = Guid.NewGuid();
            _uow.AgeRating.Add(ageRating);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(ageRating);
    }

    // GET: Admin/AgeRatings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _uow.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();
        return View(ageRating);
    }

    // POST: Admin/AgeRatings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] AgeRating ageRating)
    {
        if (id != ageRating.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _uow.AgeRating.Update(ageRating);
                await _uow.SaveChangesAsync();
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

    // GET: Admin/AgeRatings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _uow.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();

        return View(ageRating);
    }

    // POST: Admin/AgeRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.AgeRating.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> AgeRatingExists(Guid id)
    {
        return await _uow.AgeRating.ExistsAsync(id);
    }
}