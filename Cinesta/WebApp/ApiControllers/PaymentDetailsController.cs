#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples;

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
    [ProducesResponseType(typeof(object), 200)]
    [SwaggerResponseExample(200, typeof(GetPaymentDetailsExample))]
    [HttpGet]
    public async Task<object> GetUserPaymentDetails()
    {
        var res = await _public.PaymentDetails.IncludeGetByUserIdAsync(User.GetUserId());
        if (res == null) return null;
        return new
        {
            res.Id,
            res.CardType,
            res.CardNumber,
            res.ValidDate,
            res.SecurityCode,
            AppUser = new
            {
                res.AppUserId,
                res.AppUser!.Name,
                res.AppUser.Surname
            }
        };
    }
    // PUT: api/PaymentDetails/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(PaymentDetails), typeof(PostPaymentDetailsExample))]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUserPaymentDetails(Guid id, PaymentDetails paymentDetails)
    {
        if (id != paymentDetails.Id) return BadRequest();

        try
        {
            paymentDetails.AppUserId = User.GetUserId();
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
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(PaymentDetails), typeof(PostPaymentDetailsExample))]
    [SwaggerResponseExample(200, typeof(PostPaymentDetailsExample))]
    [HttpPost]
    public async Task<ActionResult<object>> PostUserPaymentDetails(PaymentDetails paymentDetails)
    {
        paymentDetails.Id = Guid.NewGuid();
        paymentDetails.AppUserId = User.GetUserId();
        _public.PaymentDetails.Add(paymentDetails);
        await _public.SaveChangesAsync();

        var res = new
        {
            paymentDetails.Id,
            paymentDetails.CardType,
            paymentDetails.CardNumber,
            paymentDetails.ValidDate,
            paymentDetails.SecurityCode,
            paymentDetails.AppUserId
        };

        return CreatedAtAction("GetUserPaymentDetails",
            new {id = paymentDetails.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, res);
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