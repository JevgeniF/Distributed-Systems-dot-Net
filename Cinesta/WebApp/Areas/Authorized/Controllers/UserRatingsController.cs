#nullable disable
using App.Contracts.DAL;
using App.Domain.Identity;
using App.Domain.Movie;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class UserRatingsController : Controller
{
    private readonly IAppUOW _uow;

    public UserRatingsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/UserRatings
    public async Task<IActionResult> Index()
    {
        return View(await _uow.UserRating.GetWithInclude());
    }

    // GET: Admin/UserRatings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _uow.UserRating.QueryableWithInclude().FirstOrDefaultAsync(m => m.Id == id);
        if (userRating == null) return NotFound();

        return View(userRating);
    }

    // GET: Admin/UserRatings/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserRatingCreateEditVM
        {
            MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
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
        if (ModelState.IsValid)
        {
            vm.UserRating.AppUserId = User.GetUserId();
            vm.UserRating.Id = Guid.NewGuid();
            _uow.UserRating.Add(vm.UserRating);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        vm.MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }

    // GET: UserRatings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _uow.UserRating.FirstOrDefaultAsync(id.Value);
        if (userRating == null) return NotFound();

        var vm = new UserRatingCreateEditVM
        {
            UserRating = userRating
        };
        vm.MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
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
            var userRatingsFromDb = await _uow.UserRating.FirstOrDefaultAsync(id);
            if (userRatingsFromDb == null) return NotFound();

            try
            {
                userRatingsFromDb.Comment.SetTranslation(userRating.Comment);
                userRating.Comment = userRatingsFromDb.Comment;

                _uow.UserRating.Update(userRating);
                await _uow.SaveChangesAsync();
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
        vm.MovieDetailsSelectList = new SelectList((await _uow.MovieDetails.GetAllAsync())
            .Select(m => new {m.Id, m.Title}), nameof(MovieDetails.Id),
            nameof(MovieDetails.Title), vm.UserRating.MovieDetailsId);
        return View(vm);
    }


    // GET: Admin/UserRatings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userRating = await _uow.UserRating.FirstOrDefaultAsync(id.Value);
        if (userRating == null) return NotFound();

        return View(userRating);
    }

    // POST: Admin/UserRatings/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.Subscription.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserRatingExists(Guid id)
    {
        return await _uow.Subscription.ExistsAsync(id);
    }
}