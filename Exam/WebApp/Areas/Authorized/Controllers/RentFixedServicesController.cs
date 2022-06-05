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
    public class RentFixedServicesController : Controller
    {
        private readonly AppDbContext _context;

        public RentFixedServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/RentFixedServices
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.RentFixedServices.Include(r => r.ApartRent).Include(r => r.FixedService);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/RentFixedServices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.RentFixedServices == null)
            {
                return NotFound();
            }

            var rentFixedService = await _context.RentFixedServices
                .Include(r => r.ApartRent)
                .Include(r => r.FixedService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentFixedService == null)
            {
                return NotFound();
            }

            return View(rentFixedService);
        }

        // GET: Authorized/RentFixedServices/Create
        public IActionResult Create()
        {
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id");
            ViewData["FixedServiceId"] = new SelectList(_context.FixedServices, "Id", "Name");
            return View();
        }

        // POST: Authorized/RentFixedServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartRentId,FixedServiceId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RentFixedService rentFixedService)
        {
            if (ModelState.IsValid)
            {
                rentFixedService.Id = Guid.NewGuid();
                _context.Add(rentFixedService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", rentFixedService.ApartRentId);
            ViewData["FixedServiceId"] = new SelectList(_context.FixedServices, "Id", "Name", rentFixedService.FixedServiceId);
            return View(rentFixedService);
        }

        // GET: Authorized/RentFixedServices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.RentFixedServices == null)
            {
                return NotFound();
            }

            var rentFixedService = await _context.RentFixedServices.FindAsync(id);
            if (rentFixedService == null)
            {
                return NotFound();
            }
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", rentFixedService.ApartRentId);
            ViewData["FixedServiceId"] = new SelectList(_context.FixedServices, "Id", "Name", rentFixedService.FixedServiceId);
            return View(rentFixedService);
        }

        // POST: Authorized/RentFixedServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartRentId,FixedServiceId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] RentFixedService rentFixedService)
        {
            if (id != rentFixedService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rentFixedService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentFixedServiceExists(rentFixedService.Id))
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
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", rentFixedService.ApartRentId);
            ViewData["FixedServiceId"] = new SelectList(_context.FixedServices, "Id", "Name", rentFixedService.FixedServiceId);
            return View(rentFixedService);
        }

        // GET: Authorized/RentFixedServices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.RentFixedServices == null)
            {
                return NotFound();
            }

            var rentFixedService = await _context.RentFixedServices
                .Include(r => r.ApartRent)
                .Include(r => r.FixedService)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rentFixedService == null)
            {
                return NotFound();
            }

            return View(rentFixedService);
        }

        // POST: Authorized/RentFixedServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.RentFixedServices == null)
            {
                return Problem("Entity set 'AppDbContext.RentFixedServices'  is null.");
            }
            var rentFixedService = await _context.RentFixedServices.FindAsync(id);
            if (rentFixedService != null)
            {
                _context.RentFixedServices.Remove(rentFixedService);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentFixedServiceExists(Guid id)
        {
          return (_context.RentFixedServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
