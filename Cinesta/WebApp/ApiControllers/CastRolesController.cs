#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain.Cast;
using WebApp.DTO;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastRolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CastRolesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/CastRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CastRoleDto>>> GetFooBars()
        {

            var res = (await _context.CastRoles.ToListAsync())
                .Select(c => new CastRoleDto()
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
            var castRole = await _context.CastRoles.FindAsync(id);

            if (castRole == null)
            {
                return NotFound();
            }

            return castRole;
        }

        // PUT: api/CastRoles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCastRole(Guid id, CastRoleDto castRole)
        {
            if (id != castRole.Id)
            {
                return BadRequest();
            }
            
            var castRoleFromDb = await _context.CastRoles.FindAsync(id);
            if (castRoleFromDb == null)
            {
                return NotFound();
            }
            
            castRoleFromDb.Naming.SetTranslation(castRole.Naming);

            _context.Entry(castRoleFromDb).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CastRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CastRoles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CastRole>> PostCastRole(CastRole castRole)
        {
            _context.CastRoles.Add(castRole);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCastRole", new { id = castRole.Id }, castRole);
        }

        // DELETE: api/CastRoles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCastRole(Guid id)
        {
            var castRole = await _context.CastRoles.FindAsync(id);
            if (castRole == null)
            {
                return NotFound();
            }

            _context.CastRoles.Remove(castRole);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CastRoleExists(Guid id)
        {
            return _context.CastRoles.Any(e => e.Id == id);
        }
    }
}
