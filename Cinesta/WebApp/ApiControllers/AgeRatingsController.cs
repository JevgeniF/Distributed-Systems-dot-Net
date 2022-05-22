#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
/// Controller for getting, adding, editing or deletion of movies age ratings system.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AgeRatingsController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    /// Movies age ratings controller's constructor.
    /// </summary>
    /// <param name="appPublic">Takes in public layer interface</param>
    public AgeRatingsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/AgeRatings
    /// <summary>
    /// Returns list of age ratings from API database.
    /// </summary>
    /// <returns>List of age ratings</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<AgeRating>), 200)]
    [HttpGet]
    public async Task<IEnumerable<AgeRating>> GetAgeRatings()
    {
        var result = await _public.AgeRating.GetAllAsync();
        return result;
    }

    // GET: api/AgeRatings/5
    /// <summary>
    /// Returns one specific age rating queried by id or error 404 if not found.
    /// </summary>
    /// <param name="id">Age rating entity id in API database</param>
    /// <returns>Age rating entity which matches to the id</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AgeRating), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<AgeRating>> GetAgeRating(Guid id)
    {
        var ageRating = await _public.AgeRating.FirstOrDefaultAsync(id);

        if (ageRating == null) return NotFound();

        return ageRating;
    }

    // PUT: api/AgeRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    /// Edits one specific age rating entity in API database or returns error 403 in case of fail.
    /// </summary>
    /// <param name="id">Age rating entity id in API database</param>
    /// <param name="ageRating">Updated age rating entity</param>
    /// <returns>Nothing</returns>
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
            _public.AgeRating.Update(ageRating);
            await _public.SaveChangesAsync();
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
    /// Adds new age rating entity to API database.
    /// </summary>
    /// <param name="ageRating">New age rating entity</param>
    /// <returns>Nothing</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(AgeRating), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<AgeRating>> PostAgeRating(AgeRating ageRating)
    {
        ageRating.Id = Guid.NewGuid();
        _public.AgeRating.Add(ageRating);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetAgeRating",
            new {id = ageRating.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()},
            ageRating);
    }

    // DELETE: api/AgeRatings/5
    /// <summary>
    /// Deletes one specific age rating entity from API database, queried by id.
    /// </summary>
    /// <param name="id">Id of age rating entity to delete</param>
    /// <returns>Nothing</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgeRating(Guid id)
    {
        await _public.AgeRating.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AgeRatingExists(Guid id)
    {
        return await _public.AgeRating.ExistsAsync(id);
    }
}