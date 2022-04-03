#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Movie;

namespace WebApp.Controllers
{
    public class MovieDbScoresController : Controller
    {
        private readonly AppDbContext _context;

        public MovieDbScoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MovieDbScores
        public async Task<IActionResult> Index()
        {
            return View(await _context.MovieDbScores.ToListAsync());
        }

        // GET: MovieDbScores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieDbScore = await _context.MovieDbScores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieDbScore == null)
            {
                return NotFound();
            }

            return View(movieDbScore);
        }

        // GET: MovieDbScores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MovieDbScores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImdbId,Score,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MovieDbScore movieDbScore)
        {
            if (ModelState.IsValid)
            {
                movieDbScore.Id = Guid.NewGuid();
                _context.Add(movieDbScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieDbScore);
        }

        // GET: MovieDbScores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieDbScore = await _context.MovieDbScores.FindAsync(id);
            if (movieDbScore == null)
            {
                return NotFound();
            }
            return View(movieDbScore);
        }

        // POST: MovieDbScores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ImdbId,Score,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")] MovieDbScore movieDbScore)
        {
            if (id != movieDbScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movieDbScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieDbScoreExists(movieDbScore.Id))
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
            return View(movieDbScore);
        }

        // GET: MovieDbScores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieDbScore = await _context.MovieDbScores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieDbScore == null)
            {
                return NotFound();
            }

            return View(movieDbScore);
        }

        // POST: MovieDbScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var movieDbScore = await _context.MovieDbScores.FindAsync(id);
            _context.MovieDbScores.Remove(movieDbScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieDbScoreExists(Guid id)
        {
            return _context.MovieDbScores.Any(e => e.Id == id);
        }
    }
}
