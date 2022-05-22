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
public class CastRolesController : Controller
{
    private readonly ILogger<CastRolesController> _logger;
    private readonly IAppPublic _public;

    public CastRolesController(IAppPublic appPublic, ILogger<CastRolesController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/CastRoles
    public async Task<IActionResult> Index()
    {
        return View(await _public.CastRole.GetAllAsync());
    }

    // GET: Authorized/CastRoles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _public.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();

        return View(castRole);
    }

    // GET: Authorized/CastRoles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Authorized/CastRoles/Create
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
            _public.CastRole.Add(castRole);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(castRole);
    }

    // GET: Authorized/CastRoles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _public.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();
        return View(castRole);
    }

    // POST: Authorized/CastRoles/Edit/5
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
            var castRoleFromDb = await _public.CastRole.FirstOrDefaultAsync(id);
            if (castRoleFromDb == null) return NotFound();

            try
            {
                castRoleFromDb.Naming.SetTranslation(castRole.Naming);
                castRole.Naming = castRoleFromDb.Naming;

                _public.CastRole.Update(castRole);
                await _public.SaveChangesAsync();
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

    // GET: Authorized/CastRoles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var castRole = await _public.CastRole.FirstOrDefaultAsync(id.Value);
        if (castRole == null) return NotFound();

        return View(castRole);
    }

    // POST: Authorized/CastRoles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.CastRole.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> CastRoleExists(Guid id)
    {
        return await _public.CastRole.ExistsAsync(id);
    }
}