#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class CastRolesController : ControllerBase
{
    private readonly IAppUOW _uow;

    public CastRolesController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/CastRoles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CastRoleDto>>> GetCastRoles()
    {
        var res = (await _uow.CastRole.GetAllAsync())
            .Select(c => new CastRoleDto
            {
                Id = c.Id,
                Naming = c.Naming
            })
            .ToList();
        return res;
    }


    // GET: api/CastRoles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CastRole>> GetCastRole(Guid id)
    {
        var castRole = await _uow.CastRole.FirstOrDefaultAsync(id);

        if (castRole == null) return NotFound();

        return castRole;
    }

    // PUT: api/CastRoles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCastRole(Guid id, CastRoleDto castRole)
    {
        if (id != castRole.Id) return BadRequest();

        var castRoleFromDb = await _uow.CastRole.FirstOrDefaultAsync(id);
        if (castRoleFromDb == null) return NotFound();

        try
        {
            castRoleFromDb.Naming.SetTranslation(castRole.Naming);
            _uow.CastRole.Update(castRoleFromDb);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<CastRole>> PostCastRole(CastRole castRole)
    {
        _uow.CastRole.Add(castRole);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetCastRole", new {id = castRole.Id}, castRole);
    }

    // DELETE: api/CastRoles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCastRole(Guid id)
    {
        await _uow.CastRole.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> CastRoleExists(Guid id)
    {
        return await _uow.CastRole.ExistsAsync(id);
    }
}