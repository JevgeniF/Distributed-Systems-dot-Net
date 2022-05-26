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
public class PaymentDetailsController : Controller
{
    private readonly ILogger<PaymentDetailsController> _logger;
    private readonly IAppPublic _public;

    public PaymentDetailsController(IAppPublic appPublic, ILogger<PaymentDetailsController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Authorized/PaymentDetails
    public async Task<IActionResult> Index()
    {
        return View(await _public.PaymentDetails.IncludeGetByUserIdAsync(User.GetUserId()));
    }

    // GET: Authorized/PaymentDetails/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _public.PaymentDetails.FirstOrDefaultAsync(id.Value);
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
        paymentDetails.AppUserId = User.GetUserId();
        if (ModelState.IsValid)
        {
            _public.PaymentDetails.Add(paymentDetails);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(paymentDetails);
    }

    // GET: PaymentDetails/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _public.PaymentDetails.FirstOrDefaultAsync(id.Value);
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
                _public.PaymentDetails.Update(paymentDetails);
                await _public.SaveChangesAsync();
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


    // GET: Authorized/PaymentDetails/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var paymentDetails = await _public.PaymentDetails.FirstOrDefaultAsync(id.Value);

        if (paymentDetails == null) return NotFound();

        return View(paymentDetails);
    }

    // POST: Authorized/PaymentDetails/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.PaymentDetails.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> PaymentDetailsExists(Guid id)
    {
        return await _public.PaymentDetails.ExistsAsync(id);
    }
}