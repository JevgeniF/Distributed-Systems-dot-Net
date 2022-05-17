#nullable disable
using App.Contracts.BLL;
using App.Public.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CastRolesController : ControllerBase
{
    private readonly IAppBll _bll;

    public CastRolesController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/CastRoles
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<CastRole>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CastRole>>> GetCastRoles()
    {
        var res = (await _bll.CastRole.GetAllAsync())
            .Select(c => new CastRole
            {
                Id = c.Id,
                Naming = c.Naming
            })
            .ToList();
        return res;
    }

    // GET: api/CastRoles/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(CastRole), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<CastRole>> GetCastRole(Guid id)
    {
        var castRole = await _bll.CastRole.FirstOrDefaultAsync(id);

        if (castRole == null) return NotFound();

        return new CastRole {Id = castRole.Id, Naming = castRole.Naming};
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

        var castRoleFromDb = await _bll.CastRole.FirstOrDefaultAsync(id);
        if (castRoleFromDb == null) return NotFound();

        try
        {
            castRoleFromDb.Naming.SetTranslation(castRole.Naming);
            _bll.CastRole.Update(castRoleFromDb);
            await _bll.SaveChangesAsync();
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
        _bll.CastRole.Add(new App.BLL.DTO.CastRole {Id = castRole.Id, Naming = castRole.Naming});
        await _bll.SaveChangesAsync();

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
        await _bll.CastRole.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CastRoleExists(Guid id)
    {
        return await _bll.CastRole.ExistsAsync(id);
    }
}