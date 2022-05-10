#nullable disable
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.BLL.DTO;
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
    private readonly IAppBll _bll;

    public AgeRatingsController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/AgeRatings
    [HttpGet]
    public async Task<IEnumerable<AgeRating>> GetAgeRatings()
    {
        var result = await _bll.AgeRating.GetAllAsync();
        return result;
    }

    // GET: api/AgeRatings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AgeRating>> GetAgeRating(Guid id)
    {
        var ageRating = await _bll.AgeRating.FirstOrDefaultAsync(id);

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
    [HttpPost]
    public async Task<ActionResult<AgeRating>> PostAgeRating(AgeRating ageRating)
    {
        _bll.AgeRating.Add(ageRating);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetAgeRating", new {id = ageRating.Id}, ageRating);
    }

    // DELETE: api/AgeRatings/5
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