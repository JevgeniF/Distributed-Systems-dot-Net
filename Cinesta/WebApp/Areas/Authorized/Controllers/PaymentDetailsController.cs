#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,user")]
public class PaymentDetailsController : Controller
{
    private readonly IAppUOW _uow;

    public PaymentDetailsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: Admin/PaymentDetails
    public async Task<IActionResult> Index()
    {
        return View(await _uow.PaymentDetails.IncludeGetAllByUserIdAsync(User.GetUserId()));
    }

    // GET: Admin/PaymentDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _uow.PaymentDetails.FirstOrDefaultAsync(id.Value);
        if (paymentDetails == null) return NotFound();

        return View(paymentDetails);
    }

    // GET: PaymentDetails/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PaymentDetails/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaymentDetails paymentDetails)
    {
        if (ModelState.IsValid)
        {
            paymentDetails.AppUserId = User.GetUserId();
            paymentDetails.Id = Guid.NewGuid();
            _uow.PaymentDetails.Add(paymentDetails);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(paymentDetails);
    }

    // GET: PaymentDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _uow.PaymentDetails.FirstOrDefaultAsync(id.Value);
        if (paymentDetails == null) return NotFound();
        return View(paymentDetails);
    }

    // POST: PaymentDetails/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, PaymentDetails paymentDetails)
    {
        if (id != paymentDetails.Id) return NotFound();

        paymentDetails.AppUserId = User.GetUserId();

        if (ModelState.IsValid)
        {
            try
            {
                _uow.PaymentDetails.Update(paymentDetails);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PaymentDetailsExists(paymentDetails.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(paymentDetails);
    }


    // GET: Admin/PaymentDetails/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _uow.PaymentDetails.FirstOrDefaultAsync(id.Value);
        if (paymentDetails == null) return NotFound();

        return View(paymentDetails);
    }

    // POST: Admin/PaymentDetails/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _uow.PaymentDetails.RemoveAsync(id);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> PaymentDetailsExists(Guid id)
    {
        return await _uow.PaymentDetails.ExistsAsync(id);
    }
}