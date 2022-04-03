#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Identity;
using App.Domain.User;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly AppDbContext _context;

        public SubscriptionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Subscriptions.Include(s => s.AppUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public async Task<IActionResult> Create() 
        { 
            var vm = new SubscriptionCreateEditVM(); 
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(), 
                nameof(AppUser.Id), nameof(AppUser.Id)); 
            return View(vm);
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubscriptionCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.Subscription);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.Subscription.AppUserId);
            return View(vm);
        }

        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            var vm = new SubscriptionCreateEditVM();
            vm.Subscription = subscription;
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.Subscription.AppUser);
            return View(vm);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Subscription subscription)
        {
            if (id != subscription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.Id))
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
            var vm = new SubscriptionCreateEditVM();
            vm.Subscription = subscription;
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.Subscription.AppUser);
            return View(vm);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // POST: Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(Guid id)
        {
            return _context.Subscriptions.Any(e => e.Id == id);
        }
    }
}
