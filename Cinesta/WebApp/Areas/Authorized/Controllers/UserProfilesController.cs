#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class UserProfilesController : Controller
{
    private readonly ILogger<UserProfilesController> _logger;
    private readonly IAppPublic _public;

    public UserProfilesController(IAppPublic appPublic, ILogger<UserProfilesController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/UserProfiles
    public async Task<IActionResult> Index()
    {
        return View(await _public.UserProfile.IncludeGetAllByUserIdAsync(User.GetUserId()));
    }

    // GET: Authorized/UserProfiles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _public.UserProfile.FirstOrDefaultAsync(id.Value);
        if (userProfile == null) return NotFound();

        return View(userProfile);
    }

    // GET: Authorized/UserProfiles/Create
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
        userProfile.AppUserId = User.GetUserId();
        if (ModelState.IsValid)
        {
            _public.UserProfile.Add(userProfile);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(userProfile);
    }

    // GET: UserProfiles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _public.UserProfile.FirstOrDefaultAsync(id.Value);
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
                _public.UserProfile.Update(userProfile);
                await _public.SaveChangesAsync();
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


    // GET: Authorized/UserProfiles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _public.UserProfile.FirstOrDefaultAsync(id.Value);
        if (userProfile == null) return NotFound();

        return View(userProfile);
    }

    // POST: Admin/UserProfiles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.UserProfile.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> UserProfileExists(Guid id)
    {
        return await _public.UserProfile.ExistsAsync(id);
    }
}