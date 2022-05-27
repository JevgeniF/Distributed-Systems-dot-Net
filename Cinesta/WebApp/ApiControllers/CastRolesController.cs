#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  CastRole entities.
///     CastRole entities meant for storage of movies roles namings.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CastRolesController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of CastRoleController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public CastRolesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/CastRoles
    /// <summary>
    ///     Method returns list of all CastRole entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of CastRole entities.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CastRole>), 200)]
    [Authorize(Roles = "admin,moderator,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public async Task<IEnumerable<CastRole>> GetCastRoles()
    {
        return await _public.CastRole.GetAllAsync();
    }

    // GET: api/CastRoles/5
    /// <summary>
    ///     Method returns one exact CastRole entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: CastRole entity Id</param>
    /// <returns>CastRole class entity</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CastRole), 200)]
    [ProducesResponseType(404)]
    [Authorize(Roles = "admin,moderator,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CastRole>> GetCastRole(Guid id)
    {
        var castRole = await _public.CastRole.FirstOrDefaultAsync(id);

        if (castRole == null) return NotFound();

        return castRole;
    }

    // PUT: api/CastRoles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits CastRole entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: CastRole entity id.</param>
    /// <param name="castRole">Updated CastRole entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCastRole(Guid id, CastRole castRole)
    {
        if (id != castRole.Id) return BadRequest();

        var castRoleFromDb = await _public.CastRole.FirstOrDefaultAsync(id);
        if (castRoleFromDb == null) return NotFound();

        try
        {
            castRoleFromDb.Naming.SetTranslation(castRole.Naming);
            _public.CastRole.Update(castRoleFromDb);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CastRoleExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/CastRoles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method adds new CastRole entity to API database
    /// </summary>
    /// <param name="castRole">CastRole class entity to add</param>
    /// <returns>Added CastRole entity with it's id </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CastRole), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<CastRole>> PostCastRole(CastRole castRole)
    {
        castRole.Id = Guid.NewGuid();
        _public.CastRole.Add(castRole);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetCastRole",
            new { id = castRole.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, castRole);
    }

    // DELETE: api/CastRoles/5
    /// <summary>
    ///     For admins and moderators only. Deletes CastRole entity found by given id.
    /// </summary>
    /// <param name="id">CastRole entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCastRole(Guid id)
    {
        await _public.CastRole.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CastRoleExists(Guid id)
    {
        return await _public.CastRole.ExistsAsync(id);
    }
}