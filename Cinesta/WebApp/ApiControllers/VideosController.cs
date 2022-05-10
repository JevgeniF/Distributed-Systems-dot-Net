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
public class VideosController : ControllerBase
{
    private readonly IAppBll _bll;

    public VideosController(IAppBll bll)
    {
        _bll = bll;
    }

    // GET: api/Videos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideos()
    {
        var res = (await _bll.Video.GetAllAsync())
            .Select(v => new VideoDto
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
    [HttpGet("{id}")]
    public async Task<ActionResult<Video>> GetVideo(Guid id)
    {
        var video = await _bll.Video.FirstOrDefaultAsync(id);

        if (video == null) return NotFound();

        return video;
    }

    // PUT: api/Videos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PutVideo(Guid id, VideoDto video)
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
    [HttpPost]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult<Video>> PostVideo(Video video)
    {
        _bll.Video.Add(video);
        await _bll.SaveChangesAsync();

        return CreatedAtAction("GetVideo", new {id = video.Id}, video);
    }

    // DELETE: api/Videos/5
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