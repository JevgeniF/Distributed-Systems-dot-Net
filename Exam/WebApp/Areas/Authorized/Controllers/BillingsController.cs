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
    public class BillingsController : Controller
    {
        private readonly AppDbContext _context;

        public BillingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/Billings
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Billings.Include(b => b.ApartRent).Include(b => b.Person);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/Billings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Billings == null)
            {
                return NotFound();
            }

            var billing = await _context.Billings
                .Include(b => b.ApartRent)
                .Include(b => b.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // GET: Authorized/Billings/Create
        public IActionResult Create()
        {
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id");
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName");
            return View();
        }

        // POST: Authorized/Billings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonId,ApartRentId,TotalSum,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Billing billing)
        {
            if (ModelState.IsValid)
            {
                billing.Id = Guid.NewGuid();
                _context.Add(billing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", billing.ApartRentId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName", billing.PersonId);
            return View(billing);
        }

        // GET: Authorized/Billings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Billings == null)
            {
                return NotFound();
            }

            var billing = await _context.Billings.FindAsync(id);
            if (billing == null)
            {
                return NotFound();
            }
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", billing.ApartRentId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName", billing.PersonId);
            return View(billing);
        }

        // POST: Authorized/Billings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PersonId,ApartRentId,TotalSum,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] Billing billing)
        {
            if (id != billing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillingExists(billing.Id))
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
            ViewData["ApartRentId"] = new SelectList(_context.ApartRents, "Id", "Id", billing.ApartRentId);
            ViewData["PersonId"] = new SelectList(_context.Persons, "Id", "FirstName", billing.PersonId);
            return View(billing);
        }

        // GET: Authorized/Billings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Billings == null)
            {
                return NotFound();
            }

            var billing = await _context.Billings
                .Include(b => b.ApartRent)
                .Include(b => b.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (billing == null)
            {
                return NotFound();
            }

            return View(billing);
        }

        // POST: Authorized/Billings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Billings == null)
            {
                return Problem("Entity set 'AppDbContext.Billings'  is null.");
            }
            var billing = await _context.Billings.FindAsync(id);
            if (billing != null)
            {
                _context.Billings.Remove(billing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillingExists(Guid id)
        {
          return (_context.Billings?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
