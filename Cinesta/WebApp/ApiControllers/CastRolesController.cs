#nullable disable
using App.BLL.DTO;
using App.Contracts.BLL;
using App.Public.DTO.v1;
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
    [ProducesResponseType(typeof(IEnumerable<App.Public.DTO.v1.CastRole>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<App.Public.DTO.v1.CastRole>>> GetCastRoles()
    {
        var res = (await _bll.CastRole.GetAllAsync())
            .Select(c => new App.Public.DTO.v1.CastRole
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
    [ProducesResponseType(typeof(App.Public.DTO.v1.CastRole), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.Public.DTO.v1.CastRole>> GetCastRole(Guid id)
    {
        var castRole = await _bll.CastRole.FirstOrDefaultAsync(id);

        if (castRole == null) return NotFound();

        return new App.Public.DTO.v1.CastRole {Id = castRole.Id, Naming = castRole.Naming};
    }

    // PUT: api/CastRoles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCastRole(Guid id, App.Public.DTO.v1.CastRole castRole)
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
    /// 
    /// </summary>
    /// <param name="castRole"></param>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.Public.DTO.v1.CastRole), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<App.Public.DTO.v1.CastRole>> PostCastRole(App.Public.DTO.v1.CastRole castRole)
    {
        _bll.CastRole.Add(new App.BLL.DTO.CastRole {Id = castRole.Id, Naming = castRole.Naming});
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetCastRole",
            new {id = castRole.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, castRole);
    }

    // DELETE: api/CastRoles/5
    /// <summary>
    /// 
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