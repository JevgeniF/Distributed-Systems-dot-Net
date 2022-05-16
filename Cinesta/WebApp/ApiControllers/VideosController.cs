#nullable disable
using App.Contracts.DAL;
using App.BLL.DTO;
using App.Contracts.BLL;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;
using Video = App.Public.DTO.v1.Video;

namespace WebApp.ApiControllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,user", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class VideosController : ControllerBase
{
    private readonly IAppBll _bll;

    public VideosController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/Videos
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Video>), 200)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Video>>> GetVideos()
    {
        var res = (await _bll.Video.GetAllAsync())
            .Select(v => new Video
            {
                Id = v.Id,
                Season = v.Season,
                Title = v.Title,
                FileUri = v.FileUri,
                Duration = v.Duration,
                Description = v.Description,
                MovieDetailsId = v.MovieDetailsId,
                MovieDetails = v.MovieDetails
            })
            .ToList();
        return res;
    }

    // GET: api/Videos/5
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.Video), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<App.BLL.DTO.Video>> GetVideo(Guid id)
    {
        var video = await _bll.Video.FirstOrDefaultAsync(id);

        if (video == null) return NotFound();

        return video;
    }

    // PUT: api/Videos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(App.BLL.DTO.Video),201)]
    [ProducesResponseType(403)]
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<App.BLL.DTO.Video>> PostVideo(App.BLL.DTO.Video video)
    {
        _bll.Video.Add(video);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetVideo", new {id = video.Id,  version = HttpContext.GetRequestedApiVersion()!.ToString()}, video);
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
        await _bll.Video.RemoveAsync(id);
        await _bll.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> VideoExists(Guid id)
    {
        return await _bll.Video.ExistsAsync(id);
    }
}