#nullable disable
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for Movie description parameter - Age Rating. Allows to create, receive, update and delete Age Rating
///     database objects. Authorized for admin usage only.
///     TODO: Think to implement moderator role and authorize moderators to use controller.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AgeRatingsController : ControllerBase
{
    private readonly IAppBll _bll;

    /// <summary>
    ///     AgeRatingsController constructor. Takes App BLL Interface as parameter.
    /// </summary>
    /// <param name="bll">IAppBLL. Supply App BLL Interface</param>
    public AgeRatingsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/AgeRatings
    /// <summary>
    ///     Get all Age Ratings from API database.
    /// </summary>
    /// <returns>Enumerator of Age Rating objects</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<AgeRating>), 200)]
    [HttpGet]
    public async Task<IEnumerable<AgeRating>> GetAgeRatings()
    {
        var result = await _bll.AgeRating.GetAllAsync();
        return result;
    }

    // GET: api/AgeRatings/5
    /// <summary>
    ///     Get single Age Rating from API database by Id.
    /// </summary>
    /// <param name="id">Guid. Age Rating database object Id</param>
    /// <returns>Age Rating object from database or Error</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AgeRating), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AgeRating>> GetAgeRating(Guid id)
    {
        var ageRating = await _bll.AgeRating.FirstOrDefaultAsync(id);

        if (ageRating == null) return NotFound();

        return ageRating;
    }

    // PUT: api/AgeRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Find Age Rating object from API database by Age Rating Id, and replace it with new or updated one.
    /// </summary>
    /// <param name="id">Guid. Age Rating database object Id</param>
    /// <param name="ageRating">
    ///     AgeRating class object. New or updated Age Rating object to save into database instead
    ///     of found one
    /// </param>
    /// <returns>No content response or Error</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAgeRating(Guid id, AgeRating ageRating)
    {
        if (id != ageRating.Id) return BadRequest();
        try
        {
            _bll.AgeRating.Update(ageRating);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AgeRatingExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/AgeRatings
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     Save new Age Rating object into database.
    /// </summary>
    /// <param name="ageRating">AgeRating class object to save into the database.</param>
    /// <returns>Saved into database Age Rating object or Error</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AgeRating), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<AgeRating>> PostAgeRating(AgeRating ageRating)
    {
        _bll.AgeRating.Add(ageRating);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetAgeRating",
            new {id = ageRating.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()},
            ageRating);
    }

    // DELETE: api/AgeRatings/5
    /// <summary>
    ///     Delete Age rating object from database by its Id.
    /// </summary>
    /// <param name="id">Age Rating object Id to find and delete from database.</param>
    /// <returns>No Content or Error.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgeRating(Guid id)
    {
        await _bll.AgeRating.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AgeRatingExists(Guid id)
    {
        return await _bll.AgeRating.ExistsAsync(id);
    }
}