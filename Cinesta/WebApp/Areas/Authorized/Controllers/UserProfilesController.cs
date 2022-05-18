#pragma warning disable CS1591
#nullable disable
using App.DAL.EF;
using App.Domain;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class UserProfilesController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserProfilesController> _logger;

    public UserProfilesController(AppDbContext context, ILogger<UserProfilesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Authorized/UserProfiles
    public async Task<IActionResult> Index()
    {
        return View(await _context.UserProfiles.Include(u => u.AppUser)
            .Where(u => u.AppUserId == User.GetUserId()).ToListAsync());
    }

    // GET: Authorized/UserProfiles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
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
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(userProfile);
    }

    // GET: UserProfiles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(u => u.Id == id);
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
                _context.UserProfiles.Update(userProfile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(userProfile.Id))
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

        var userProfile = await _context.UserProfiles
            .Include(u => u.AppUser)
            .FirstOrDefaultAsync(u => u.Id == id);
        if (userProfile == null) return NotFound();

        return View(userProfile);
    }

    // POST: Admin/UserProfiles/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var userProfile = await _context.UserProfiles.FindAsync(id);
        _context.UserProfiles.Remove(userProfile!);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserProfileExists(Guid id)
    {
        return _context.UserProfiles.Any(u => u.Id == id);
    }
}