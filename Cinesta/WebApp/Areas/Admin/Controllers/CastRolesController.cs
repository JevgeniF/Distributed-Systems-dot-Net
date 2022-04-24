#nullable disable
using App.Contracts.DAL;
using App.Domain.Cast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
public class CastRolesController : Controller
{
    private readonly IAppUOW _uow;

    public CastRolesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/CastRoles
    public async Task<IActionResult> Index()
    {
        return View(await _uow.CastRole.GetAllAsync());
    }

    // GET: Admin/CastRoles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _uow.CastRole.FirstOrDefaultAsync(id.Value);

        if (castRole == null) return NotFound();

        return View(castRole);
    }

    // GET: Admin/CastRoles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/CastRoles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] CastRole castRole)
    {
        if (ModelState.IsValid)
        {
            castRole.Id = Guid.NewGuid();
            _uow.CastRole.Add(castRole);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(castRole);
    }

    // GET: Admin/CastRoles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _uow.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();
        return View(castRole);
    }

    // POST: Admin/CastRoles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] CastRole castRole)
    {
        if (id != castRole.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var castRoleFromDb = await _uow.CastRole.FirstOrDefaultAsync(id);
            if (castRoleFromDb == null) return NotFound();

            try
            {
                castRoleFromDb.Naming.SetTranslation(castRole.Naming);
                castRole.Naming = castRoleFromDb.Naming;

                _uow.CastRole.Update(castRole);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CastRoleExists(castRole.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(castRole);
    }

    // GET: Admin/CastRoles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _uow.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();

        return View(castRole);
    }

    // POST: Admin/CastRoles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.CastRole.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CastRoleExists(Guid id)
    {
        return await _uow.CastRole.ExistsAsync(id);
    }
}