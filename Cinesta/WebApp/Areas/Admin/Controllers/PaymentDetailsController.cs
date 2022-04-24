#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Identity;
using App.Domain.User;
using WebApp.Areas.Admin.ViewModels;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PaymentDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public PaymentDetailsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PaymentDetails
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.PaymentDetails.Include(p => p.AppUser);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/PaymentDetails/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentDetails = await _context.PaymentDetails
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentDetails == null)
            {
                return NotFound();
            }

            return View(paymentDetails);
        }
        
        // GET: PaymentDetails/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PaymentDetailsCreateEditVM();
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id));
            return View(vm);
        }

        // POST: PaymentDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentDetailsCreateEditVM vm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vm.PaymentDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.PaymentDetails.AppUserId);
            return View(vm);
        }

        // GET: PaymentDetails/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentDetails = await _context.PaymentDetails.FindAsync(id);
            if (paymentDetails == null)
            {
                return NotFound();
            }
            var vm = new PaymentDetailsCreateEditVM();
            vm.PaymentDetails = paymentDetails;
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.PaymentDetails.AppUser);
            return View(vm);
        }

        // POST: PaymentDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PaymentDetails paymentDetails)
        {
            if (id != paymentDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paymentDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentDetailsExists(paymentDetails.Id))
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
            var vm = new PaymentDetailsCreateEditVM();
            vm.PaymentDetails = paymentDetails;
            vm.AppUserSelectList = new SelectList(
                await _context.Users.Select(u => new {u.Id}).ToListAsync(),
                nameof(AppUser.Id), nameof(AppUser.Id), vm.PaymentDetails.AppUser);
            return View(vm);
        }



        // GET: Admin/PaymentDetails/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paymentDetails = await _context.PaymentDetails
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paymentDetails == null)
            {
                return NotFound();
            }

            return View(paymentDetails);
        }

        // POST: Admin/PaymentDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var paymentDetails = await _context.PaymentDetails.FindAsync(id);
            _context.PaymentDetails.Remove(paymentDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentDetailsExists(Guid id)
        {
            return _context.PaymentDetails.Any(e => e.Id == id);
        }
    }
}
