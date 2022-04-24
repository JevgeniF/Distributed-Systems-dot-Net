#nullable disable
using App.DAL.EF;
using App.Domain.Identity;
using App.Domain.Profile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers;

[Area("Admin")]
public class UserProfilesController : Controller
{
    private readonly AppDbContext _context;

    public UserProfilesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Admin/UserProfiles
    public async Task<IActionResult> Index()
    {
        var appDbContext = _context.UserProfiles.Include(u => u.AppUser);
        return View(await appDbContext.ToListAsync());
    }

    // GET: Admin/UserProfiles/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _context.UserProfiles
            .Include(u => u.AppUser)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (userProfile == null) return NotFound();

        return View(userProfile);
    }

    // GET: Admin/UserProfiles/Create
    public async Task<IActionResult> Create()
    {
        var vm = new UserProfileCreateEditVM();
        vm.AppUserSelectList = new SelectList(
            await _context.Users.Select(u => new {u.Id}).ToListAsync(),
            nameof(AppUser.Id), nameof(AppUser.Id));
        return View(vm);
    }

    // POST: UserProfiles/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserProfileCreateEditVM vm)
    {
        if (ModelState.IsValid)
        {
            _context.Add(vm.UserProfile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.AppUserSelectList = new SelectList(
            await _context.Users.Select(u => new {u.Id}).ToListAsync(),
            nameof(AppUser.Id), nameof(AppUser.Id), vm.UserProfile.AppUserId);
        return View(vm);
    }

    // GET: UserProfiles/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _context.UserProfiles.FindAsync(id);
        if (userProfile == null) return NotFound();
        var vm = new UserProfileCreateEditVM();
        vm.UserProfile = userProfile;
        vm.AppUserSelectList = new SelectList(
            await _context.Users.Select(u => new {u.Id}).ToListAsync(),
            nameof(AppUser.Id), nameof(AppUser.Id), vm.UserProfile.AppUserId);
        return View(vm);
    }

    // POST: UserProfiles/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UserProfile userProfile)
    {
        if (id != userProfile.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(userProfile);
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

        var vm = new UserProfileCreateEditVM();
        vm.UserProfile = userProfile;
        vm.AppUserSelectList = new SelectList(
            await _context.Users.Select(u => new {u.Id}).ToListAsync(),
            nameof(AppUser.Id), nameof(AppUser.Id), vm.UserProfile.AppUserId);
        return View(vm);
    }


    // GET: Admin/UserProfiles/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var userProfile = await _context.UserProfiles
            .Include(u => u.AppUser)
            .FirstOrDefaultAsync(m => m.Id == id);
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
        _context.UserProfiles.Remove(userProfile);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool UserProfileExists(Guid id)
    {
        return _context.UserProfiles.Any(e => e.Id == id);
    }
}