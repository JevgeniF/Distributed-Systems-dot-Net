#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Cast;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CastRolesController : Controller
    {
        private readonly AppDbContext _context;

        public CastRolesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CastRoles
        public async Task<IActionResult> Index()
        {
            return View(await _context.CastRoles.ToListAsync());
        }

        // GET: Admin/CastRoles/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castRole = await _context.CastRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castRole == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> Create([Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] CastRole castRole)
        {
            if (ModelState.IsValid)
            {
                castRole.Id = Guid.NewGuid();
                _context.Add(castRole);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(castRole);
        }

        // GET: Admin/CastRoles/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castRole = await _context.CastRoles.FindAsync(id);
            if (castRole == null)
            {
                return NotFound();
            }
            return View(castRole);
        }

        // POST: Admin/CastRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] CastRole castRole)
        {
            if (id != castRole.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var castRoleFromDb = await _context.CastRoles.AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == castRole.Id);
                if (castRoleFromDb == null)
                {
                    return NotFound();
                }
                
                try
                {
                    castRoleFromDb.Naming.SetTranslation(castRole.Naming);
                    castRole.Naming = castRoleFromDb.Naming;
                    
                    _context.Update(castRole);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastRoleExists(castRole.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(castRole);
        }

        // GET: Admin/CastRoles/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castRole = await _context.CastRoles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castRole == null)
            {
                return NotFound();
            }

            return View(castRole);
        }

        // POST: Admin/CastRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var castRole = await _context.CastRoles.FindAsync(id);
            _context.CastRoles.Remove(castRole);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastRoleExists(Guid id)
        {
            return _context.CastRoles.Any(e => e.Id == id);
        }
    }
}
