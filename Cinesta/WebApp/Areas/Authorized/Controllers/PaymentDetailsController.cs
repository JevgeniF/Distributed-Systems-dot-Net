#nullable disable
#pragma warning disable CS1591
using App.BLL.DTO;
using App.Contracts.BLL;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator,user")]
public class PaymentDetailsController : Controller
{
    private readonly IAppBll _bll;

    public PaymentDetailsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: Admin/PaymentDetails
    public async Task<IActionResult> Index()
    {
        return View(await _bll.PaymentDetails.IncludeGetByUserIdAsync(User.GetUserId()));
    }

    // GET: Admin/PaymentDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _bll.PaymentDetails.FirstOrDefaultAsync(id.Value);
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
            _bll.PaymentDetails.Add(paymentDetails);
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(paymentDetails);
    }

    // GET: PaymentDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _bll.PaymentDetails.FirstOrDefaultAsync(id.Value);
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
                _bll.PaymentDetails.Update(paymentDetails);
                await _bll.SaveChangesAsync();
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

        var paymentDetails = await _bll.PaymentDetails.FirstOrDefaultAsync(id.Value);
        if (paymentDetails == null) return NotFound();

        return View(paymentDetails);
    }

    // POST: Admin/PaymentDetails/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _bll.PaymentDetails.RemoveAsync(id);
        await _bll.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> PaymentDetailsExists(Guid id)
    {
        return await _bll.PaymentDetails.ExistsAsync(id);
    }
}