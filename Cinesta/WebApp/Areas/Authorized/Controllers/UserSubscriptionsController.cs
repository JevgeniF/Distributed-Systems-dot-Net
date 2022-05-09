#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Mvc;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApp.Areas.Authorized.ViewModels;

namespace WebApp.Areas.Authorized.Controllers
{
    [Area("Authorized")]
    [Authorize(Roles = "admin,user")]
    public class UserSubscriptionsController : Controller
    {
        private readonly IAppUOW _uow;

        public UserSubscriptionsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: Authorized/UserSubscriptions
        public async Task<IActionResult> Index()
        {
            return View(await _uow.UserSubscription.IncludeGetAllByUserIdAsync(User.GetUserId()));
        }

        // GET: Authorized/UserSubscriptions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var userSubscription =
                await _uow.UserSubscription.IncludeFirstOrDefaultAsync(id.Value);
            if (userSubscription == null) return NotFound();

            return View(userSubscription);
        }

        // GET: Authorized/UserSubscriptions/Create
        public async Task<IActionResult> Create()
        {
            var vm = new UserSubscriptionCreateVM
            {
                SubscriptionSelectList = new SelectList((await _uow.Subscription.GetAllAsync())
                    .Select(s => new {s.Id, s.Naming}), nameof(Subscription.Id),
                    nameof(Subscription.Naming))
            };
            return View(vm);
        }

        // POST: Authorized/UserSubscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserSubscriptionCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.UserSubscription.AppUserId = User.GetUserId();
                _uow.UserSubscription.Add(vm.UserSubscription);
                await _uow.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            vm.SubscriptionSelectList = new SelectList((await _uow.Subscription.GetAllAsync())
                .Select(s => new {s.Id, s.Naming}), nameof(Subscription.Id),
                nameof(Subscription.Naming), vm.UserSubscription.SubscriptionId);
            return View(vm);
        }

        // GET: Authorized/UserSubscriptions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var userSubscription =
                await _uow.UserSubscription.IncludeFirstOrDefaultAsync(id.Value);
            if (userSubscription == null) return NotFound();

            return View(userSubscription);
        }

        // POST: Authorized/UserSubscriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _uow.UserSubscription.RemoveAsync(id);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserSubscriptionExists(Guid id)
        {
            return await _uow.UserSubscription.ExistsAsync(id);
        }
    }
}
