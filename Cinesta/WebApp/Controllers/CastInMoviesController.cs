#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Cast;
using App.Domain.Common;
using App.Domain.Movie;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class CastInMoviesController : Controller
    {
        private readonly AppDbContext _context;

        public CastInMoviesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: CastInMovies
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.CastInMovies.Include(c => c.CastRole).Include(c => c.MovieDetails).Include(c => c.Persons);
            return View(await appDbContext.ToListAsync());
        }

        // GET: CastInMovies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castInMovie = await _context.CastInMovies
                .Include(c => c.CastRole)
                .Include(c => c.MovieDetails)
                .Include(c => c.Persons)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castInMovie == null)
            {
                return NotFound();
            }

            return View(castInMovie);
        }

        // GET: CastInMovies/Create
        public async Task<IActionResult> Create()
        {
            var vm = new CastInMovieCreateEditVM();
            vm.CastRoleSelectList = new SelectList(
                await _context.CastRoles.Select(c => new {c.Id, c.Naming}).ToListAsync(),
                nameof(CastRole.Id), nameof(CastRole.Naming));
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title));
            vm.PersonSelectList = new SelectList(
                await _context.Persons.Select(p => new {p.Id, p.Name})
                    .ToListAsync(), nameof(Person.Id),
                nameof(Person.Name));
            return View(vm);
        }

        // POST: CastInMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // TODO: NAME + SURNAME in Person Select List
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CastInMovieCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.CastInMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.CastRoleSelectList = new SelectList(
                await _context.CastRoles.Select(c => new {c.Id, c.Naming}).ToListAsync(),
                nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie.CastRoleId);
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
            vm.PersonSelectList = new SelectList(
                await _context.Persons.Select(p => new {p.Id, p.Name})
                    .ToListAsync(), nameof(Person.Id),
                nameof(Person.Name), vm.CastInMovie.PersonId);
            return View(vm);
        }

        // GET: CastInMovies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castInMovie = await _context.CastInMovies.FindAsync(id);
            if (castInMovie == null)
            {
                return NotFound();
            }
            var vm = new CastInMovieCreateEditVM();
            vm.CastInMovie = castInMovie;
            vm.CastRoleSelectList = new SelectList(
                await _context.CastRoles.Select(c => new {c.Id, c.Naming}).ToListAsync(),
                nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie.CastRoleId);
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
            vm.PersonSelectList = new SelectList(
                await _context.Persons.Select(p => new {p.Id, p.Name})
                    .ToListAsync(), nameof(Person.Id),
                nameof(Person.Name), vm.CastInMovie.PersonId);
            return View(vm);
        }

        // POST: CastInMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CastInMovie castInMovie)
        {
            if (id != castInMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(castInMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CastInMovieExists(castInMovie.Id))
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
            var vm = new CastInMovieCreateEditVM();
            vm.CastInMovie = castInMovie;
            vm.CastRoleSelectList = new SelectList(
                await _context.CastRoles.Select(c => new {c.Id, c.Naming}).ToListAsync(),
                nameof(CastRole.Id), nameof(CastRole.Naming), vm.CastInMovie.CastRoleId);
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.CastInMovie.MovieDetailsId);
            vm.PersonSelectList = new SelectList(
                await _context.Persons.Select(p => new {p.Id, p.Name})
                    .ToListAsync(), nameof(Person.Id),
                nameof(Person.Name), vm.CastInMovie.PersonId);
            return View(vm);
        }

        // GET: CastInMovies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var castInMovie = await _context.CastInMovies
                .Include(c => c.CastRole)
                .Include(c => c.MovieDetails)
                .Include(c => c.Persons)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (castInMovie == null)
            {
                return NotFound();
            }

            return View(castInMovie);
        }

        // POST: CastInMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var castInMovie = await _context.CastInMovies.FindAsync(id);
            _context.CastInMovies.Remove(castInMovie!);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CastInMovieExists(Guid id)
        {
            return _context.CastInMovies.Any(e => e.Id == id);
        }
    }
}
