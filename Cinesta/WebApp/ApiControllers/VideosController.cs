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
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VideosController : ControllerBase
{
    private readonly IAppPublic _public;

    public VideosController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/Videos
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetListVideosExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetVideos()
    {
        return (await _public.Video.IncludeGetAllAsync())
            .Select(v => new
            {
                v.Id,
                v.Season,
                v.Title,
                v.FileUri,
                v.Duration,
                v.Description,
                MovieDetails = new
                {
                    v.MovieDetailsId,
                    v.MovieDetails!.Title
                }
            });
    }

    // GET: api/Videos/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetVideoExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetVideo(Guid id)
    {
        var video = await _public.Video.FirstOrDefaultAsync(id);

        if (video == null) return NotFound();

        return new
        {
            video.Id,
            video.Season,
            video.Title,
            video.FileUri,
            video.Duration,
            video.Description,
            MovieDetails = new
            {
                video.MovieDetailsId,
                video.MovieDetails!.Title
            }
        };
    }

    // PUT: api/Videos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(Video), typeof(PostVideoExample))]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutVideo(Guid id, Video video)
    {
        if (id != video.Id) return BadRequest();

        var videoFromDb = await _public.Video.FirstOrDefaultAsync(id);
        if (videoFromDb == null) return NotFound();

        try
        {
            videoFromDb.Title.SetTranslation(video.Title);
            videoFromDb.Description.SetTranslation(video.Description);
            _public.Video.Update(videoFromDb);
            await _public.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await VideoExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Videos
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(Video), typeof(PostVideoExample))]
    [SwaggerResponseExample(200, typeof(PostVideoExample))]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<object>> PostVideo(Video video)
    {
        video.Id = Guid.NewGuid();
        _public.Video.Add(video);
        await _public.SaveChangesAsync();
        var res = new
        {
            video.Id,
            video.Season,
            video.Title,
            video.FileUri,
            video.Duration,
            video.Description,
            video.MovieDetailsId
        };

        return CreatedAtAction("GetVideo",
            new {id = video.Id, version = HttpContext.GetRequestedApiVersion()!.ToString()}, res);
    }

    // DELETE: api/Videos/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteVideo(Guid id)
    {
        await _public.Video.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> VideoExists(Guid id)
    {
        return await _public.Video.ExistsAsync(id);
    }
}