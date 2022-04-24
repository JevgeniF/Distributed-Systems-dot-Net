#nullable disable
using App.Contracts.DAL;
using App.Domain.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[ApiController]
public class VideosController : ControllerBase
{
    private readonly IAppUOW _uow;

    public VideosController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/Videos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideos()
    {
        var res = (await _uow.Video.GetAllAsync())
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
        var video = await _uow.Video.FirstOrDefaultAsync(id);

        if (video == null) return NotFound();

        return video;
    }

    // PUT: api/Videos/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutVideo(Guid id, VideoDto video)
    {
        if (id != video.Id) return BadRequest();

        var videoFromDb = await _uow.Video.FirstOrDefaultAsync(id);
        if (videoFromDb == null) return NotFound();

        try
        {
            videoFromDb.Title.SetTranslation(video.Title);
            videoFromDb.Description.SetTranslation(video.Description);
            _uow.Video.Update(videoFromDb);
            await _uow.SaveChangesAsync();
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
    public async Task<ActionResult<Video>> PostVideo(Video video)
    {
        _uow.Video.Add(video);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetVideo", new {id = video.Id}, video);
    }

    // DELETE: api/Videos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVideo(Guid id)
    {
        await _uow.Video.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> VideoExists(Guid id)
    {
        return await _uow.Video.ExistsAsync(id);
    }
}