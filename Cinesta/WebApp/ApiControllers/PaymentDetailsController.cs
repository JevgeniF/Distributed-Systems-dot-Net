#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using App.Public.DTO.v1.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin, user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PaymentDetailsController : ControllerBase
{
    private readonly IAppPublic _public;

    public PaymentDetailsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/PaymentDetails
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<PaymentDetails>), 200)]
    [HttpGet()]
    public async Task<IEnumerable<PaymentDetails>> GetUserPaymentDetails()
    {
        return await _public.PaymentDetails.IncludeGetAllByUserIdAsync(User.GetUserId());
    }

    // GET: api/PaymentDetails/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PaymentDetails), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentDetails>> GetPaymentDetails(Guid id)
    {
        var paymentDetails = await _public.PaymentDetails.FirstOrDefaultAsync(id);

        if (paymentDetails == null) return NotFound();

        return paymentDetails;
    }

    // PUT: api/PaymentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPaymentDetails(Guid id, PaymentDetails paymentDetails)
    {
        if (id != paymentDetails.Id) return BadRequest();

        try
        {
            _public.PaymentDetails.Update(paymentDetails);
            await _public.SaveChangesAsync();
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(PaymentDetails), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<PaymentDetails>> PostPaymentDetails(PaymentDetails paymentDetails)
    {
        paymentDetails.Id = Guid.NewGuid();
        paymentDetails.AppUserId = User.GetUserId();
        _public.PaymentDetails.Add(paymentDetails);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetPaymentDetails",
            new {id = paymentDetails.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, paymentDetails);
    }

    // DELETE: api/PaymentDetails/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaymentDetails(Guid id)
    {
        await _public.PaymentDetails.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> PaymentDetailsExists(Guid id)
    {
        return await _public.PaymentDetails.ExistsAsync(id);
    }
}