#nullable disable
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain.Movie;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class VideosController : Controller
{
    private readonly IAppUOW _uow;

    public VideosController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/Videos
    public async Task<IActionResult> Index()
    {
        return View(await _uow.Video.GetWithInclude());
    }

    // GET: Admin/Videos/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var video = await _uow.Video.QueryableWithInclude().FirstOrDefaultAsync(m => m.Id == id);
        if (video == null) return NotFound();

        return View(video);
    }

    // GET: Admin/Videos/Create
    public async Task<IActionResult> Create()
    {
        var vm = new VideoCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
                .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
        };
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
            _uow.Video.Add(vm.Video);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
        return View(vm);
    }

    // GET: Videos/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var video = await _uow.Video.FirstOrDefaultAsync(id.Value);
        if (video == null) return NotFound();

        var vm = new VideoCreateEditVM
        {
            Video = video
        };
        vm.MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
        return View(vm);
    }

    // POST: Videos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, Video video)
    {
        if (id != video.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var videoFromDb = await _uow.Video.FirstOrDefaultAsync(id);
            if (videoFromDb == null) return NotFound();

            try
            {
                videoFromDb.Title.SetTranslation(video.Title);
                video.Title = videoFromDb.Title;

                videoFromDb.Description.SetTranslation(video.Description);
                video.Description = videoFromDb.Description;

                _uow.Video.Update(video);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VideoExists(video.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new VideoCreateEditVM();
        vm.MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
        return View(vm);
    }

    // GET: Admin/Videos/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var video = await _uow.Video.QueryableWithInclude().FirstOrDefaultAsync(m => m.Id == id);
        if (video == null) return NotFound();

        return View(video);
    }

    // POST: Admin/Videos/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.Video.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> VideoExists(Guid id)
    {
        return await _uow.Video.ExistsAsync(id);
    }
}