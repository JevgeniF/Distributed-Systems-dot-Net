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
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class VideosController : Controller
    {
        private readonly AppDbContext _context;

        public VideosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Videos.Include(v => v.MovieDetails);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        public async Task<IActionResult> Create()
        {
            var vm = new VideoCreateEditVM();
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title));
            return View(vm);
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.Video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
            return View(vm);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            var vm = new VideoCreateEditVM();
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
            return View(vm);

        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Video video)
        {
            if (id != video.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.Id))
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
            var vm = new VideoCreateEditVM();
            vm.MovieDetailsSelectList = new SelectList(
                await _context.MovieDetails.Select(m => new {m.Id, m.Title}).ToListAsync(),
                nameof(MovieDetails.Id), nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
            return View(vm);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Videos
                .Include(v => v.MovieDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var video = await _context.Videos.FindAsync(id);
            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(Guid id)
        {
            return _context.Videos.Any(e => e.Id == id);
        }
    }
}
