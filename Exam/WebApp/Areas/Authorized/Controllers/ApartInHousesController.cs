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
    public class ApartInHousesController : Controller
    {
        private readonly AppDbContext _context;

        public ApartInHousesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/ApartInHouses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ApartInHouse.Include(a => a.Apartment).Include(a => a.House);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/ApartInHouses/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ApartInHouse == null)
            {
                return NotFound();
            }

            var apartInHouse = await _context.ApartInHouse
                .Include(a => a.Apartment)
                .Include(a => a.House)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartInHouse == null)
            {
                return NotFound();
            }

            return View(apartInHouse);
        }

        // GET: Authorized/ApartInHouses/Create
        public IActionResult Create()
        {
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id");
            ViewData["HouseId"] = new SelectList(_context.Houses, "Id", "Address");
            return View();
        }

        // POST: Authorized/ApartInHouses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HouseId,ApartmentId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartInHouse apartInHouse)
        {
            if (ModelState.IsValid)
            {
                apartInHouse.Id = Guid.NewGuid();
                _context.Add(apartInHouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartInHouse.ApartmentId);
            ViewData["HouseId"] = new SelectList(_context.Houses, "Id", "Address", apartInHouse.HouseId);
            return View(apartInHouse);
        }

        // GET: Authorized/ApartInHouses/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ApartInHouse == null)
            {
                return NotFound();
            }

            var apartInHouse = await _context.ApartInHouse.FindAsync(id);
            if (apartInHouse == null)
            {
                return NotFound();
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartInHouse.ApartmentId);
            ViewData["HouseId"] = new SelectList(_context.Houses, "Id", "Address", apartInHouse.HouseId);
            return View(apartInHouse);
        }

        // POST: Authorized/ApartInHouses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("HouseId,ApartmentId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartInHouse apartInHouse)
        {
            if (id != apartInHouse.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartInHouse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartInHouseExists(apartInHouse.Id))
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
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartInHouse.ApartmentId);
            ViewData["HouseId"] = new SelectList(_context.Houses, "Id", "Address", apartInHouse.HouseId);
            return View(apartInHouse);
        }

        // GET: Authorized/ApartInHouses/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ApartInHouse == null)
            {
                return NotFound();
            }

            var apartInHouse = await _context.ApartInHouse
                .Include(a => a.Apartment)
                .Include(a => a.House)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartInHouse == null)
            {
                return NotFound();
            }

            return View(apartInHouse);
        }

        // POST: Authorized/ApartInHouses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ApartInHouse == null)
            {
                return Problem("Entity set 'AppDbContext.ApartInHouse'  is null.");
            }
            var apartInHouse = await _context.ApartInHouse.FindAsync(id);
            if (apartInHouse != null)
            {
                _context.ApartInHouse.Remove(apartInHouse);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartInHouseExists(Guid id)
        {
          return (_context.ApartInHouse?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
