#nullable disable
using App.Contracts.DAL;
using App.Domain.Profile;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class UserProfilesController : Controller
{
    private readonly IAppUOW _uow;

    public UserProfilesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/UserProfiles
    public async Task<IActionResult> Index()
    {
        return View(await _uow.UserProfile.GetAllByUserIdAsync(User.GetUserId()));
    }

    // GET: Admin/UserProfiles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _uow.UserProfile.FirstOrDefaultAsync(id.Value);
        if (userProfile == null) return NotFound();

        return View(userProfile);
    }

    // GET: Admin/UserProfiles/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: UserProfiles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserProfile userProfile)
    {
        if (ModelState.IsValid)
        {
            userProfile.AppUserId = User.GetUserId();
            userProfile.Id = Guid.NewGuid();
            _uow.UserProfile.Add(userProfile);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(userProfile);
    }

    // GET: UserProfiles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _uow.UserProfile.FirstOrDefaultAsync(id.Value);
        if (userProfile == null) return NotFound();
        return View(userProfile);
    }

    // POST: UserProfiles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserProfile userProfile)
    {
        if (id != userProfile.Id) return NotFound();

        userProfile.AppUserId = User.GetUserId();

        if (ModelState.IsValid)
        {
            try
            {
                _uow.UserProfile.Update(userProfile);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserProfileExists(userProfile.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(userProfile);
    }


    // GET: Admin/UserProfiles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _uow.UserProfile.FirstOrDefaultAsync(id.Value);
        if (userProfile == null) return NotFound();

        return View(userProfile);
    }

    // POST: Admin/UserProfiles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.UserProfile.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserProfileExists(Guid id)
    {
        return await _uow.UserProfile.ExistsAsync(id);
    }
}