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
    public class ApartPicturesController : Controller
    {
        private readonly AppDbContext _context;

        public ApartPicturesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Authorized/ApartPictures
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.ApartPictures.Include(a => a.Apartment).Include(a => a.Picture);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Authorized/ApartPictures/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ApartPictures == null)
            {
                return NotFound();
            }

            var apartPicture = await _context.ApartPictures
                .Include(a => a.Apartment)
                .Include(a => a.Picture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartPicture == null)
            {
                return NotFound();
            }

            return View(apartPicture);
        }

        // GET: Authorized/ApartPictures/Create
        public IActionResult Create()
        {
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id");
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "PictureUri");
            return View();
        }

        // POST: Authorized/ApartPictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApartmentId,PictureId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartPicture apartPicture)
        {
            if (ModelState.IsValid)
            {
                apartPicture.Id = Guid.NewGuid();
                _context.Add(apartPicture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartPicture.ApartmentId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "PictureUri", apartPicture.PictureId);
            return View(apartPicture);
        }

        // GET: Authorized/ApartPictures/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ApartPictures == null)
            {
                return NotFound();
            }

            var apartPicture = await _context.ApartPictures.FindAsync(id);
            if (apartPicture == null)
            {
                return NotFound();
            }
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartPicture.ApartmentId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "PictureUri", apartPicture.PictureId);
            return View(apartPicture);
        }

        // POST: Authorized/ApartPictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ApartmentId,PictureId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] ApartPicture apartPicture)
        {
            if (id != apartPicture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(apartPicture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApartPictureExists(apartPicture.Id))
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
            ViewData["ApartmentId"] = new SelectList(_context.Apartments, "Id", "Id", apartPicture.ApartmentId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "PictureUri", apartPicture.PictureId);
            return View(apartPicture);
        }

        // GET: Authorized/ApartPictures/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ApartPictures == null)
            {
                return NotFound();
            }

            var apartPicture = await _context.ApartPictures
                .Include(a => a.Apartment)
                .Include(a => a.Picture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (apartPicture == null)
            {
                return NotFound();
            }

            return View(apartPicture);
        }

        // POST: Authorized/ApartPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ApartPictures == null)
            {
                return Problem("Entity set 'AppDbContext.ApartPictures'  is null.");
            }
            var apartPicture = await _context.ApartPictures.FindAsync(id);
            if (apartPicture != null)
            {
                _context.ApartPictures.Remove(apartPicture);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApartPictureExists(Guid id)
        {
          return (_context.ApartPictures?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
