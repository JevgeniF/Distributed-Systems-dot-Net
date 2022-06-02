#nullable disable
using App.Contracts.BLL;
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using WebApp.SwaggerExamples.Videos;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with Video entities.
///     Video entities meant for storage of movie video file link and data.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VideosController : ControllerBase
{
    private readonly IAppPublic _public;
    private readonly IAppBll _bll;

    /// <summary>
    ///     Constructor of VideosController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public VideosController(IAppPublic appPublic, IAppBll bll)
    {
        _public = appPublic;
        _bll = bll;
    }

    // GET: api/Videos
    /// <summary>
    ///     Method returns list of all Video entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of generated from Video entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<object>), 200)]
    [SwaggerResponseExample(200, typeof(GetListVideosExample))]
    [HttpGet]
    public async Task<IEnumerable<object>> GetVideos(string culture)
    {
        return (await _bll.Video.IncludeGetAllAsync())
            .Select(v => new
            {
                v.Id,
                v.Season,
                Title = v.Title.Translate(culture),
                v.FileUri,
                v.Duration,
                Description = v.Description.Translate(culture),
                MovieDetails = new
                {
                    Id = v.MovieDetailsId,
                    Title = v.MovieDetails!.Title.Translate(culture)
                }
            });
    }

    // GET: api/Videos/5
    /// <summary>
    ///     Method returns one exact Video entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: Video entity Id</param>
    /// <returns>Generated from Video entity object</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(404)]
    [SwaggerResponseExample(200, typeof(GetVideoExample))]
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetVideo(Guid id, string culture)
    {
        var video = await _bll.Video.FirstOrDefaultAsync(id);

        if (video == null) return NotFound();

        return new
        {
            video.Id,
            video.Season,
            Title = video.Title.Translate(culture),
            video.FileUri,
            video.Duration,
            Description = video.Description.Translate(culture),
            MovieDetails = new
            {
                Id = video.MovieDetailsId,
                Title = video.MovieDetails!.Title.Translate(culture)
            }
        };
    }

    // PUT: api/Videos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits Video entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: Video entity id.</param>
    /// <param name="video">Updated Video entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [SwaggerRequestExample(typeof(Video), typeof(PostVideoExample))]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutVideo(Guid id, Video video)
    {
        if (id != video.Id) return BadRequest();

        var videoFromDb = await _bll.Video.FirstOrDefaultAsync(id);
        if (videoFromDb == null) return NotFound();

        try
        {
            videoFromDb.Title.SetTranslation(video.Title);
            videoFromDb.Description.SetTranslation(video.Description);
            _bll.Video.Update(videoFromDb);
            await _bll.SaveChangesAsync();
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
    /// <summary>
    ///     For admins and moderators only. Method adds new Video entity to API database
    /// </summary>
    /// <param name="video">Video class entity to add</param>
    /// <returns>Generated from Video entity object </returns>
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
            video.MovieDetailsId
        };

        return CreatedAtAction("GetVideo",
            new { id = video.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, res);
    }

    // DELETE: api/Videos/5
    /// <summary>
    ///     For admins and moderators only. Deletes Video entity found by given id.
    /// </summary>
    /// <param name="id">Video entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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