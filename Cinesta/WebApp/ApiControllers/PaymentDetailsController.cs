#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.PaymentDetails;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  PaymentDetails entities.
///     PaymentDetails entities meant for storage of user payment data for future communication with Payment Service Api.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PaymentDetailsController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of PaymentDetailsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public PaymentDetailsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/PaymentDetails
    /// <summary>
    ///     Method returns current user PaymentDetails entity stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from PaymentDetails entity object</returns>
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
    /// <summary>
    ///     Method edits PaymentDetails entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: PaymentDetails entity id.</param>
    /// <param name="paymentDetails">Updated PaymentDetails entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
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
    /// <summary>
    ///     Method adds new PaymentDetails entity for current user. Only one PaymentDetails entity per user allowed!
    ///     If entity exists, returns error.
    /// </summary>
    /// <param name="paymentDetails">PaymentDetails class entity to add</param>
    /// <returns>Generated from PaymentDetails entity object </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(PaymentDetails), typeof(PostPaymentDetailsExample))]
    [SwaggerResponseExample(200, typeof(PostPaymentDetailsExample))]
    [HttpPost]
    public async Task<ActionResult<object>> PostUserPaymentDetails(PaymentDetails paymentDetails)
    {
        if (await GetUserPaymentDetails() != null) return StatusCode(403);
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
            new { id = paymentDetails.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, res);
    }

    // DELETE: api/PaymentDetails/5
    /// <summary>
    ///     Deletes PaymentDetails entity found by given id.
    /// </summary>
    /// <param name="id">PaymentDetails entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
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