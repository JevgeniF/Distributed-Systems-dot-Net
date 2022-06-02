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
public class CastRolesController : Controller
{
    private readonly IAppBll _bll;

    public CastRolesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/CastRoles
    public async Task<IActionResult> Index()
    {
        return View(await _bll.CastRole.GetAllAsync());
    }

    // GET: Admin/CastRoles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _bll.CastRole.FirstOrDefaultAsync(id.Value);

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
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        CastRole castRole)
    {
        if (ModelState.IsValid)
        {
            castRole.Id = Guid.NewGuid();
            _bll.CastRole.Add(castRole);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(castRole);
    }

    // GET: Admin/CastRoles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _bll.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();
        return View(castRole);
    }

    // POST: Admin/CastRoles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        CastRole castRole)
    {
        if (id != castRole.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var castRoleFromDb = await _bll.CastRole.FirstOrDefaultAsync(id);
            if (castRoleFromDb == null) return NotFound();

            try
            {
                castRoleFromDb.Naming.SetTranslation(castRole.Naming);
                castRole.Naming = castRoleFromDb.Naming;

                _bll.CastRole.Update(castRole);
                await _bll.SaveChangesAsync();
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

        var castRole = await _bll.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();

        return View(castRole);
    }

    // POST: Admin/CastRoles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.CastRole.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CastRoleExists(Guid id)
    {
        return await _bll.CastRole.ExistsAsync(id);
    }
}