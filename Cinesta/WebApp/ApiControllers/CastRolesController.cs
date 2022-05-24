#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
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
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CastRolesController : ControllerBase
{
    private readonly IAppPublic _public;

    public CastRolesController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/CastRoles
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CastRole>), 200)]
    [HttpGet]
    public async Task<IEnumerable<CastRole>> GetCastRoles()
    {
        return await _public.CastRole.GetAllAsync();
        
    }

    // GET: api/CastRoles/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CastRole), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CastRole>> GetCastRole(Guid id)
    {
        var castRole = await _public.CastRole.FirstOrDefaultAsync(id);

        if (castRole == null) return NotFound();

        return castRole;
    }

    // PUT: api/CastRoles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
    /// </summary>
    /// <param name="castRole"></param>
    /// <returns></returns>
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
            new {id = castRole.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, castRole);
    }

    // DELETE: api/CastRoles/5
    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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