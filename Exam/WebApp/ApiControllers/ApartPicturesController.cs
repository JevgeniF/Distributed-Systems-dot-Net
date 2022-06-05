using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Contracts.DAL;
using App.DAL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartPicturesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public ApartPicturesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/ApartPictures
        [HttpGet]
        public async Task<IEnumerable<App.DAL.DTO.ApartPicture>> GetApartPictures()
        {
            return await _uow.ApartPicture.GetAllAsync();
        }
        
        // GET: api/ApartPictures/apart
        [HttpGet("apart={apartId}")]
        public async Task<IEnumerable<App.DAL.DTO.ApartPicture>> GetApartPictures(Guid apartId)
        {
            return await _uow.ApartPicture.GetAllByApartId(apartId, true);
        }

        // GET: api/ApartPictures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartPicture>> GetApartPicture(Guid id)
        {
            var apartPicture = await _uow.ApartPicture.FirstOrDefaultAsync(id);

            if (apartPicture == null)
            {
                return NotFound();
            }

            return apartPicture;
        }

        // PUT: api/ApartPictures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartPicture(Guid id, ApartPicture apartPicture)
        {
            if (id != apartPicture.Id)
            {
                return BadRequest();
            }

            try
            {
                _uow.ApartPicture.Update(apartPicture);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApartPictureExists(id))
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

        // POST: api/ApartPictures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApartPicture>> PostApartPicture(ApartPicture apartPicture)
        {
            _uow.ApartPicture.Add(apartPicture);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetApartPicture", new { id = apartPicture.Id }, apartPicture);
        }

        // DELETE: api/ApartPictures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartPicture(Guid id)
        {
            await _uow.ApartPicture.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ApartPictureExists(Guid id)
        {
            return await _uow.ApartPicture.ExistsAsync(id);
        }
    }
}
