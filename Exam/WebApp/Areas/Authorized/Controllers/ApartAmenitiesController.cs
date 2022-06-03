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
    public class ApartAmenitiesController : Controller
    {
        private readonly AppDbContext _context;

        public ApartAmenitiesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/ApartAmenities
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ApartAmenities.Include(a => a.Amenity).Include(a => a.Apartment);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/ApartAmenities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ApartAmenities == null)
            {
                return NotFound();
            }

            var apartAmenity = await _context.ApartAmenities
                .Include(a => a.Amenity)
                .Include(a => a.Apartment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartAmenity == null)
            {
                return NotFound();
            }

            return View(apartAmenity);
        }

        // GET: Authorized/ApartAmenities/Create
        public IActionResult Create()
        {
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "Id", "Name");
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id");
            return View();
        }

        // POST: Authorized/ApartAmenities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartmentId,AmenityId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartAmenity apartAmenity)
        {
            if (ModelState.IsValid)
            {
                apartAmenity.Id = Guid.NewGuid();
                _context.Add(apartAmenity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "Id", "Name", apartAmenity.AmenityId);
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartAmenity.ApartmentId);
            return View(apartAmenity);
        }

        // GET: Authorized/ApartAmenities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ApartAmenities == null)
            {
                return NotFound();
            }

            var apartAmenity = await _context.ApartAmenities.FindAsync(id);
            if (apartAmenity == null)
            {
                return NotFound();
            }
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "Id", "Name", apartAmenity.AmenityId);
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartAmenity.ApartmentId);
            return View(apartAmenity);
        }

        // POST: Authorized/ApartAmenities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartmentId,AmenityId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartAmenity apartAmenity)
        {
            if (id != apartAmenity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartAmenity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartAmenityExists(apartAmenity.Id))
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
            ViewData["AmenityId"] = new SelectList(_context.Amenities, "Id", "Name", apartAmenity.AmenityId);
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartAmenity.ApartmentId);
            return View(apartAmenity);
        }

        // GET: Authorized/ApartAmenities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ApartAmenities == null)
            {
                return NotFound();
            }

            var apartAmenity = await _context.ApartAmenities
                .Include(a => a.Amenity)
                .Include(a => a.Apartment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartAmenity == null)
            {
                return NotFound();
            }

            return View(apartAmenity);
        }

        // POST: Authorized/ApartAmenities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ApartAmenities == null)
            {
                return Problem("Entity set 'AppDbContext.ApartAmenities'  is null.");
            }
            var apartAmenity = await _context.ApartAmenities.FindAsync(id);
            if (apartAmenity != null)
            {
                _context.ApartAmenities.Remove(apartAmenity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartAmenityExists(Guid id)
        {
          return (_context.ApartAmenities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
