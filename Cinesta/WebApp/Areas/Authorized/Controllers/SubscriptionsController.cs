#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Authorized.Controllers
{
    [Area("Authorized")]
    [Authorize(Roles = "admin,user")]
    public class SubscriptionsController : Controller
    {
        private readonly IAppUOW _uow;

        public SubscriptionsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: Authorized/Subscriptions
        public async Task<IActionResult> Index()
        {
            return View(await _uow.Subscription.GetAllAsync());
        }

        // GET: Authorized/Subscriptions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var subscription = await _uow.Subscription.FirstOrDefaultAsync(id.Value);
            if (subscription == null) return NotFound();

            return View(subscription);
        }

        // GET: Authorized/Subscriptions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authorized/Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Naming,Description,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
            Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                subscription.Id = Guid.NewGuid();
                _uow.Subscription.Add(subscription);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subscription);
        }

        // GET: Authorized/Subscriptions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var subscription = await _uow.Subscription.FirstOrDefaultAsync(id.Value);
            if (subscription == null) return NotFound();
            return View(subscription);
        }

        // POST: Authorized/Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id,
            [Bind("Naming,Description,Price,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
            Subscription subscription)
        {
            if (id != subscription.Id) return NotFound();
            
            if (ModelState.IsValid)
            {
                try
                {
                    _uow.Subscription.Update(subscription);
                    await _uow.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await SubscriptionExists(subscription.Id))
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
            return View(subscription);
        }

        // GET: Authorized/Subscriptions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var subscription = await _uow.Subscription.FirstOrDefaultAsync(id.Value);
            if (subscription == null) return NotFound();

            return View(subscription);
        }

        // POST: Authorized/Subscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.Subscription.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SubscriptionExists(Guid id)
        {
            return await _uow.Subscription.ExistsAsync(id);
        }
    }
}
