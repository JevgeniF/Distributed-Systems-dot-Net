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
    public class ApartRentsController : Controller
    {
        private readonly AppDbContext _context;

        public ApartRentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/ApartRents
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ApartRents.Include(a => a.Apartment).Include(a => a.Person);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/ApartRents/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ApartRents == null)
            {
                return NotFound();
            }

            var apartRent = await _context.ApartRents
                .Include(a => a.Apartment)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartRent == null)
            {
                return NotFound();
            }

            return View(apartRent);
        }

        // GET: Authorized/ApartRents/Create
        public IActionResult Create()
        {
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id");
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName");
            return View();
        }

        // POST: Authorized/ApartRents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartmentId,PersonId,RentMonth,RentYear,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartRent apartRent)
        {
            if (ModelState.IsValid)
            {
                apartRent.Id = Guid.NewGuid();
                _context.Add(apartRent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartRent.ApartmentId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName", apartRent.PersonId);
            return View(apartRent);
        }

        // GET: Authorized/ApartRents/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ApartRents == null)
            {
                return NotFound();
            }

            var apartRent = await _context.ApartRents.FindAsync(id);
            if (apartRent == null)
            {
                return NotFound();
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartRent.ApartmentId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName", apartRent.PersonId);
            return View(apartRent);
        }

        // POST: Authorized/ApartRents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartmentId,PersonId,RentMonth,RentYear,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartRent apartRent)
        {
            if (id != apartRent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartRent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartRentExists(apartRent.Id))
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
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartRent.ApartmentId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName", apartRent.PersonId);
            return View(apartRent);
        }

        // GET: Authorized/ApartRents/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ApartRents == null)
            {
                return NotFound();
            }

            var apartRent = await _context.ApartRents
                .Include(a => a.Apartment)
                .Include(a => a.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartRent == null)
            {
                return NotFound();
            }

            return View(apartRent);
        }

        // POST: Authorized/ApartRents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ApartRents == null)
            {
                return Problem("Entity set 'AppDbContext.ApartRents'  is null.");
            }
            var apartRent = await _context.ApartRents.FindAsync(id);
            if (apartRent != null)
            {
                _context.ApartRents.Remove(apartRent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartRentExists(Guid id)
        {
          return (_context.ApartRents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
