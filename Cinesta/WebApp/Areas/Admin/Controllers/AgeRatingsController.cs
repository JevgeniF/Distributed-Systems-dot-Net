#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.MovieStandardDetails;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgeRatingsController : Controller
    {
        private readonly AppDbContext _context;

        public AgeRatingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/AgeRatings
        public async Task<IActionResult> Index()
        {
            return View(await _context.AgeRatings.ToListAsync());
        }

        // GET: Admin/AgeRatings/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ageRating = await _context.AgeRatings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ageRating == null)
            {
                return NotFound();
            }

            return View(ageRating);
        }

        // GET: Admin/AgeRatings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AgeRatings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] AgeRating ageRating)
        {
            if (ModelState.IsValid)
            {
                ageRating.Id = Guid.NewGuid();
                _context.Add(ageRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ageRating);
        }

        // GET: Admin/AgeRatings/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ageRating = await _context.AgeRatings.FindAsync(id);
            if (ageRating == null)
            {
                return NotFound();
            }
            return View(ageRating);
        }

        // POST: Admin/AgeRatings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Naming,AllowedAge,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] AgeRating ageRating)
        {
            if (id != ageRating.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ageRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgeRatingExists(ageRating.Id))
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
            return View(ageRating);
        }

        // GET: Admin/AgeRatings/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ageRating = await _context.AgeRatings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ageRating == null)
            {
                return NotFound();
            }

            return View(ageRating);
        }

        // POST: Admin/AgeRatings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ageRating = await _context.AgeRatings.FindAsync(id);
            _context.AgeRatings.Remove(ageRating);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AgeRatingExists(Guid id)
        {
            return _context.AgeRatings.Any(e => e.Id == id);
        }
    }
}
