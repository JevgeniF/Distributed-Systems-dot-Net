#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class UserRatingsController : Controller
{
    private readonly ILogger<UserRatingsController> _logger;
    private readonly IAppPublic _public;

    public UserRatingsController(IAppPublic appPublic, ILogger<UserRatingsController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/UserRatings
    public async Task<IActionResult> Index()
    {
        return View(await _public.UserRating.IncludeGetAllAsync());
    }

    // GET: Authorized/UserRatings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _public.UserRating.IncludeFirstOrDefaultAsync(id.Value);
        if (userRating == null) return NotFound();

        return View(userRating);
    }


    // GET: Authorized/UserRatings/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserRatingCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _public.MovieDetails.GetAllAsync())
                .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
                nameof(MovieDetails.Title))
        };
        return View(vm);
    }

    // POST: UserRatings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserRatingCreateEditVM vm)
    {
        vm.UserRating.AppUserId = User.GetUserId();
        if (ModelState.IsValid)
        {
            _public.UserRating.Add(vm.UserRating);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.MovieDetailsSelectList = new SelectList((await _public.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }

    // GET: UserRatings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();
        var userRating = await _public.UserRating.FirstOrDefaultAsync(id.Value);
        if (userRating == null) return NotFound();

        var vm = new UserRatingCreateEditVM
        {
            UserRating = userRating
        };
        vm.MovieDetailsSelectList = new SelectList((await _public.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }

    // POST: UserRatings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserRating userRating)
    {
        if (id != userRating.Id) return NotFound();
        userRating.AppUserId = User.GetUserId();

        if (ModelState.IsValid)
        {
            var userRatingsFromDb = await _public.UserRating.FirstOrDefaultAsync(id);
            if (userRatingsFromDb == null) return NotFound();

            try
            {
                userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
                userRating.Comment = userRatingsFromDb.Comment;

                _public.UserRating.Update(userRating);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserRatingExists(userRating.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        var vm = new UserRatingCreateEditVM
        {
            UserRating = userRating
        };
        vm.MovieDetailsSelectList = new SelectList((await _public.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }


    // GET: Admin/UserRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _public.UserRating.IncludeFirstOrDefaultAsync(id.Value);
        if (userRating == null) return NotFound();

        return View(userRating);
    }

    // POST: Admin/UserRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.UserRating.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserRatingExists(Guid id)
    {
        return await _public.UserRating.ExistsAsync(id);
    }
}