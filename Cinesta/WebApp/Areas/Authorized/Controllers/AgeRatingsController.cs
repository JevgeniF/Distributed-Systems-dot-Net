#nullable disable
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.BLL.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin")]
public class AgeRatingsController : Controller
{
    private readonly IAppBll _bll;

    public AgeRatingsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/AgeRatings
    public async Task<IActionResult> Index()
    {
        var result = await _bll.AgeRating.GetAllAsync();
        return View(result);
    }

    // GET: Admin/AgeRatings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _bll.AgeRating.FirstOrDefaultAsync(id.Value);
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
        [Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        AgeRating ageRating)
    {
        if (ModelState.IsValid)
        {
            ageRating.Id = Guid.NewGuid();
            _bll.AgeRating.Add(ageRating);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(ageRating);
    }

    // GET: Admin/AgeRatings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var ageRating = await _bll.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();
        return View(ageRating);
    }

    // POST: Admin/AgeRatings/Edit/5
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
                _bll.AgeRating.Update(ageRating);
                await _bll.SaveChangesAsync();
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

        var ageRating = await _bll.AgeRating.FirstOrDefaultAsync(id.Value);
        if (ageRating == null) return NotFound();

        return View(ageRating);
    }

    // POST: Admin/AgeRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.AgeRating.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> AgeRatingExists(Guid id)
    {
        return await _bll.AgeRating.ExistsAsync(id);
    }
}