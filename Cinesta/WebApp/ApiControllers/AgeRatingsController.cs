#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class AgeRatingsController : ControllerBase
{
    private readonly IAppUOW _uow;

    public AgeRatingsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/AgeRatings
    [HttpGet]
    public async Task<IEnumerable<AgeRating>> GetAgeRatings()
    {
        var result = await _uow.AgeRating.GetAllAsync();
        return result;
    }

    // GET: api/AgeRatings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AgeRating>> GetAgeRating(Guid id)
    {
        var ageRating = await _uow.AgeRating.FirstOrDefaultAsync(id);

        if (ageRating == null) return NotFound();

        return ageRating;
    }

    // PUT: api/AgeRatings/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAgeRating(Guid id, AgeRating ageRating)
    {
        if (id != ageRating.Id) return BadRequest();
        try
        {
            _uow.AgeRating.Update(ageRating);
            await _uow.SaveChangesAsync();
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
    [HttpPost]
    public async Task<ActionResult<AgeRating>> PostAgeRating(AgeRating ageRating)
    {
        _uow.AgeRating.Add(ageRating);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetAgeRating", new {id = ageRating.Id}, ageRating);
    }

    // DELETE: api/AgeRatings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAgeRating(Guid id)
    {
        await _uow.AgeRating.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> AgeRatingExists(Guid id)
    {
        return await _uow.AgeRating.ExistsAsync(id);
    }
}