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
    public class MovieTypesController : Controller
    {
        private readonly AppDbContext _context;

        public MovieTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MovieTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.MovieTypes.ToListAsync());
        }

        // GET: Admin/MovieTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieType = await _context.MovieTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieType == null)
            {
                return NotFound();
            }

            return View(movieType);
        }

        // GET: Admin/MovieTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MovieTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MovieType movieType)
        {
            if (ModelState.IsValid)
            {
                movieType.Id = Guid.NewGuid();
                _context.Add(movieType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieType);
        }

        // GET: Admin/MovieTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieType = await _context.MovieTypes.FindAsync(id);
            if (movieType == null)
            {
                return NotFound();
            }
            return View(movieType);
        }

        // POST: Admin/MovieTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Naming,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MovieType movieType)
        {
            if (id != movieType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieTypeExists(movieType.Id))
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
            return View(movieType);
        }

        // GET: Admin/MovieTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieType = await _context.MovieTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieType == null)
            {
                return NotFound();
            }

            return View(movieType);
        }

        // POST: Admin/MovieTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movieType = await _context.MovieTypes.FindAsync(id);
            _context.MovieTypes.Remove(movieType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieTypeExists(Guid id)
        {
            return _context.MovieTypes.Any(e => e.Id == id);
        }
    }
}
