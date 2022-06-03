using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using App.DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public PicturesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/Pictures
        [HttpGet]
        public async Task<IEnumerable<Picture>> GetPictures()
        {
            return await _uow.Picture.GetAllAsync();
        }

        // GET: api/Pictures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Picture>> GetPicture(Guid id)
        {
            var picture = await _uow.Picture.FirstOrDefaultAsync(id);

            if (picture == null)
            {
                return NotFound();
            }

            return picture;
        }

        // PUT: api/Pictures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPicture(Guid id, Picture picture)
        {
            if (id != picture.Id)
            {
                return BadRequest();
            }
            try
            {
                _uow.Picture.Update(picture);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PictureExists(id))
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

        // POST: api/Pictures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Picture>> PostPicture(Picture picture)
        {
            _uow.Picture.Add(picture);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetPicture", new { id = picture.Id }, picture);
        }

        // DELETE: api/Pictures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(Guid id)
        {
            await _uow.Picture.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> PictureExists(Guid id)
        {
            return await _uow.Picture.ExistsAsync(id);
        }
    }
}
