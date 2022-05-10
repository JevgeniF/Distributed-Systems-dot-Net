#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class MovieTypesController : ControllerBase
{
    private readonly IAppBll _bll;

    public MovieTypesController(IAppBll uow)
    {
        _bll = uow;
    }

    // GET: api/MovieTypes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieTypeDto>>> GetMovieTypes()
    {
        var res = (await _bll.MovieType.GetAllAsync())
            .Select(m => new MovieTypeDto
            {
                Id = m.Id,
                Naming = m.Naming
            })
            .ToList();
        return res;
    }

    // GET: api/MovieTypes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieType>> GetMovieType(Guid id)
    {
        var movieType = await _bll.MovieType.FirstOrDefaultAsync(id);

        if (movieType == null) return NotFound();

        return movieType;
    }

    // PUT: api/MovieTypes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutMovieType(Guid id, MovieTypeDto movieType)
    {
        if (id != movieType.Id) return BadRequest();

        var movieTypeFromDb = await _bll.MovieType.FirstOrDefaultAsync(id);
        if (movieTypeFromDb == null) return NotFound();

        try
        {
            movieTypeFromDb.Naming.SetTranslation(movieType.Naming);
            _bll.MovieType.Update(movieTypeFromDb);
            await _bll.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MovieTypeExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/MovieTypes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<MovieType>> PostMovieType(MovieType movieType)
    {
        _bll.MovieType.Add(movieType);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetMovieType", new {id = movieType.Id}, movieType);
    }

    // DELETE: api/MovieTypes/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteMovieType(Guid id)
    {
        _bll.MovieType.Remove(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> MovieTypeExists(Guid id)
    {
        return await _bll.MovieType.ExistsAsync(id);
    }
}