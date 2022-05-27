#nullable disable
#pragma warning disable CS1591
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class VideosController : Controller
{
    private readonly IAppBll _bll;

    public VideosController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/Videos
    public async Task<IActionResult> Index()
    {
        return View(await _bll.Video.IncludeGetAllAsync());
    }

    // GET: Admin/Videos/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var video = await _bll.Video.IncludeFirstOrDefaultAsync(id.Value);
        if (video == null) return NotFound();

        return View(video);
    }

    // GET: Admin/Videos/Create
    public async Task<IActionResult> Create()
    {
        var vm = new VideoCreateEditVm
        {
            MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
                .Select(m => new { m.Id, m.Title }), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
        };
        return View(vm);
    }

    // POST: Videos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(VideoCreateEditVm vm)
    {
        if (ModelState.IsValid)
        {
            _bll.Video.Add(vm.Video);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(m => new { m.Id, m.Title }), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
        return View(vm);
    }

    // GET: Videos/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var video = await _bll.Video.FirstOrDefaultAsync(id.Value);
        if (video == null) return NotFound();

        var vm = new VideoCreateEditVm
        {
            Video = video
        };
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(m => new { m.Id, m.Title }), nameof(MovieDetails.Id),
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
            var videoFromDb = await _bll.Video.FirstOrDefaultAsync(id);
            if (videoFromDb == null) return NotFound();

            try
            {
                videoFromDb.Title.SetTranslation(video.Title);
                video.Title = videoFromDb.Title;

                videoFromDb.Description.SetTranslation(video.Description);
                video.Description = videoFromDb.Description;

                _bll.Video.Update(video);
                await _bll.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await VideoExists(video.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new VideoCreateEditVm();
        vm.MovieDetailsSelectList = new SelectList((await _bll.MovieDetails.GetAllAsync())
            .Select(m => new { m.Id, m.Title }), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.Video.MovieDetailsId);
        return View(vm);
    }

    // GET: Admin/Videos/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var video = await _bll.Video.IncludeFirstOrDefaultAsync(id.Value);
        if (video == null) return NotFound();

        return View(video);
    }

    // POST: Admin/Videos/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.Video.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> VideoExists(Guid id)
    {
        return await _bll.Video.ExistsAsync(id);
    }
}