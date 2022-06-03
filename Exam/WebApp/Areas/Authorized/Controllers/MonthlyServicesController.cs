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
    public class MonthlyServicesController : Controller
    {
        private readonly AppDbContext _context;

        public MonthlyServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/MonthlyServices
        public async Task<IActionResult> Index()
        {
              return _context.MonthlyServices != null ? 
                          View(await _context.MonthlyServices.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.MonthlyServices'  is null.");
        }

        // GET: Authorized/MonthlyServices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.MonthlyServices == null)
            {
                return NotFound();
            }

            var monthlyService = await _context.MonthlyServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monthlyService == null)
            {
                return NotFound();
            }

            return View(monthlyService);
        }

        // GET: Authorized/MonthlyServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authorized/MonthlyServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MonthlyService monthlyService)
        {
            if (ModelState.IsValid)
            {
                monthlyService.Id = Guid.NewGuid();
                _context.Add(monthlyService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monthlyService);
        }

        // GET: Authorized/MonthlyServices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.MonthlyServices == null)
            {
                return NotFound();
            }

            var monthlyService = await _context.MonthlyServices.FindAsync(id);
            if (monthlyService == null)
            {
                return NotFound();
            }
            return View(monthlyService);
        }

        // POST: Authorized/MonthlyServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MonthlyService monthlyService)
        {
            if (id != monthlyService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monthlyService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyServiceExists(monthlyService.Id))
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
            return View(monthlyService);
        }

        // GET: Authorized/MonthlyServices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.MonthlyServices == null)
            {
                return NotFound();
            }

            var monthlyService = await _context.MonthlyServices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monthlyService == null)
            {
                return NotFound();
            }

            return View(monthlyService);
        }

        // POST: Authorized/MonthlyServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.MonthlyServices == null)
            {
                return Problem("Entity set 'AppDbContext.MonthlyServices'  is null.");
            }
            var monthlyService = await _context.MonthlyServices.FindAsync(id);
            if (monthlyService != null)
            {
                _context.MonthlyServices.Remove(monthlyService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MonthlyServiceExists(Guid id)
        {
          return (_context.MonthlyServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
