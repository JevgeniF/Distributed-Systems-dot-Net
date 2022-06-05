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
    public class RentMonthlyServicesController : Controller
    {
        private readonly AppDbContext _context;

        public RentMonthlyServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/RentMonthlyServices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.RentMonthlyServices.Include(r => r.ApartRent).Include(r => r.MonthlyService);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/RentMonthlyServices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.RentMonthlyServices == null)
            {
                return NotFound();
            }

            var rentMonthlyService = await _context.RentMonthlyServices
                .Include(r => r.ApartRent)
                .Include(r => r.MonthlyService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentMonthlyService == null)
            {
                return NotFound();
            }

            return View(rentMonthlyService);
        }

        // GET: Authorized/RentMonthlyServices/Create
        public IActionResult Create()
        {
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id");
            ViewData["MonthlyServiceId"] = new SelectList(_context.MonthlyServices, "Id", "Name");
            return View();
        }

        // POST: Authorized/RentMonthlyServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartRentId,MonthlyServiceId,Consumption,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RentMonthlyService rentMonthlyService)
        {
            if (ModelState.IsValid)
            {
                rentMonthlyService.Id = Guid.NewGuid();
                _context.Add(rentMonthlyService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", rentMonthlyService.ApartRentId);
            ViewData["MonthlyServiceId"] = new SelectList(_context.MonthlyServices, "Id", "Name", rentMonthlyService.MonthlyServiceId);
            return View(rentMonthlyService);
        }

        // GET: Authorized/RentMonthlyServices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.RentMonthlyServices == null)
            {
                return NotFound();
            }

            var rentMonthlyService = await _context.RentMonthlyServices.FindAsync(id);
            if (rentMonthlyService == null)
            {
                return NotFound();
            }
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", rentMonthlyService.ApartRentId);
            ViewData["MonthlyServiceId"] = new SelectList(_context.MonthlyServices, "Id", "Name", rentMonthlyService.MonthlyServiceId);
            return View(rentMonthlyService);
        }

        // POST: Authorized/RentMonthlyServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartRentId,MonthlyServiceId,Consumption,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RentMonthlyService rentMonthlyService)
        {
            if (id != rentMonthlyService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentMonthlyService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentMonthlyServiceExists(rentMonthlyService.Id))
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
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", rentMonthlyService.ApartRentId);
            ViewData["MonthlyServiceId"] = new SelectList(_context.MonthlyServices, "Id", "Name", rentMonthlyService.MonthlyServiceId);
            return View(rentMonthlyService);
        }

        // GET: Authorized/RentMonthlyServices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.RentMonthlyServices == null)
            {
                return NotFound();
            }

            var rentMonthlyService = await _context.RentMonthlyServices
                .Include(r => r.ApartRent)
                .Include(r => r.MonthlyService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentMonthlyService == null)
            {
                return NotFound();
            }

            return View(rentMonthlyService);
        }

        // POST: Authorized/RentMonthlyServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.RentMonthlyServices == null)
            {
                return Problem("Entity set 'AppDbContext.RentMonthlyServices'  is null.");
            }
            var rentMonthlyService = await _context.RentMonthlyServices.FindAsync(id);
            if (rentMonthlyService != null)
            {
                _context.RentMonthlyServices.Remove(rentMonthlyService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentMonthlyServiceExists(Guid id)
        {
          return (_context.RentMonthlyServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
