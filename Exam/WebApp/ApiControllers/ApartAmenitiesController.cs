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
    public class ApartAmenitiesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public ApartAmenitiesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/ApartAmenities
        [HttpGet]
        public async Task<IEnumerable<App.DAL.DTO.ApartAmenity>> GetApartAmenities()
        {
            return await _uow.ApartAmenity.GetAllAsync();
        }
        
        // GET: api/ApartAmenities/apart
        [HttpGet("apart={id}")]
        public async Task<IEnumerable<App.DAL.DTO.ApartAmenity>> GetApartAmenitiesForOneApart(Guid id)
        {
            return await _uow.ApartAmenity.GetAllByApartId(id, true);
        }

        // GET: api/ApartAmenities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartAmenity>> GetApartAmenity(Guid id)
        {

            var apartAmenity = await _uow.ApartAmenity.FirstOrDefaultAsync(id);

            if (apartAmenity == null)
            {
                return NotFound();
            }

            return apartAmenity;
        }

        // PUT: api/ApartAmenities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartAmenity(Guid id, ApartAmenity apartAmenity)
        {
            if (id != apartAmenity.Id)
            {
                return BadRequest();
            }

            try
            {
                _uow.ApartAmenity.Update(apartAmenity);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApartAmenityExists(id))
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

        // POST: api/ApartAmenities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApartAmenity>> PostApartAmenity(ApartAmenity apartAmenity)
        {
            _uow.ApartAmenity.Add(apartAmenity);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetApartAmenity", new { id = apartAmenity.Id }, apartAmenity);
        }

        // DELETE: api/ApartAmenities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartAmenity(Guid id)
        {
            await _uow.ApartAmenity.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ApartAmenityExists(Guid id)
        {
            return await _uow.ApartAmenity.ExistsAsync(id);
        }
    }
}
