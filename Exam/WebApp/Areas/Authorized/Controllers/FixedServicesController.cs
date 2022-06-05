using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas.Authorized.Controllers
{
    [Area("Authorized")]
    public class FixedServicesController : Controller
    {
        private readonly AppDbContext _context;

        public FixedServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/FixedServices
        public async Task<IActionResult> Index()
        {
              return _context.FixedServices != null ? 
                          View(await _context.FixedServices.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.FixedServices'  is null.");
        }

        // GET: Authorized/FixedServices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.FixedServices == null)
            {
                return NotFound();
            }

            var fixedService = await _context.FixedServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixedService == null)
            {
                return NotFound();
            }

            return View(fixedService);
        }

        // GET: Authorized/FixedServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authorized/FixedServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] FixedService fixedService)
        {
            if (ModelState.IsValid)
            {
                fixedService.Id = Guid.NewGuid();
                _context.Add(fixedService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(fixedService);
        }

        // GET: Authorized/FixedServices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.FixedServices == null)
            {
                return NotFound();
            }

            var fixedService = await _context.FixedServices.FindAsync(id);
            if (fixedService == null)
            {
                return NotFound();
            }
            return View(fixedService);
        }

        // POST: Authorized/FixedServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] FixedService fixedService)
        {
            if (id != fixedService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixedService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixedServiceExists(fixedService.Id))
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
            return View(fixedService);
        }

        // GET: Authorized/FixedServices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.FixedServices == null)
            {
                return NotFound();
            }

            var fixedService = await _context.FixedServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fixedService == null)
            {
                return NotFound();
            }

            return View(fixedService);
        }

        // POST: Authorized/FixedServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.FixedServices == null)
            {
                return Problem("Entity set 'AppDbContext.FixedServices'  is null.");
            }
            var fixedService = await _context.FixedServices.FindAsync(id);
            if (fixedService != null)
            {
                _context.FixedServices.Remove(fixedService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixedServiceExists(Guid id)
        {
          return (_context.FixedServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
