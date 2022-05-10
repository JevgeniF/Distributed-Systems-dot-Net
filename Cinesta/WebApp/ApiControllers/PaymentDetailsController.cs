#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin, user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class PaymentDetailsController : ControllerBase
{
    private readonly IAppBll _bll;

    public PaymentDetailsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/PaymentDetails
    [HttpGet]
    public async Task<IEnumerable<PaymentDetails>> GetPaymentDetails()
    {
        return await _bll.PaymentDetails.GetAllAsync();
    }

    // GET: api/PaymentDetails/5
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDetails>> GetPaymentDetails(Guid id)
    {
        var paymentDetails = await _bll.PaymentDetails.FirstOrDefaultAsync(id);

        if (paymentDetails == null) return NotFound();

        return paymentDetails;
    }

    // PUT: api/PaymentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPaymentDetails(Guid id, PaymentDetails paymentDetails)
    {
        if (id != paymentDetails.Id) return BadRequest();

        try
        {
            _bll.PaymentDetails.Update(paymentDetails);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PaymentDetailsExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/PaymentDetails
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<PaymentDetails>> PostPaymentDetails(PaymentDetails paymentDetails)
    {
        _bll.PaymentDetails.Add(paymentDetails);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetPaymentDetails", new {id = paymentDetails.Id}, paymentDetails);
    }

    // DELETE: api/PaymentDetails/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaymentDetails(Guid id)
    {
        await _bll.PaymentDetails.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> PaymentDetailsExists(Guid id)
    {
        return await _bll.PaymentDetails.ExistsAsync(id);
    }
}