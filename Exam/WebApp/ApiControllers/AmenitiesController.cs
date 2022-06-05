using App.Contracts.DAL;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public AmenitiesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/Amenities
        [HttpGet]
        public async Task<IEnumerable<App.DAL.DTO.Amenity>> GetAmenities()
        {
            return await _uow.Amenity.GetAllAsync();
        }

        // GET: api/Amenities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<App.DAL.DTO.Amenity>> GetAmenity(Guid id)
        {
            var amenity = await _uow.Amenity.FirstOrDefaultAsync(id);

            if (amenity == null)
            {
                return NotFound();
            }

            return amenity;
        }

        // PUT: api/Amenities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAmenity(Guid id, Amenity amenity)
        {
            if (id != amenity.Id)
            {
                return BadRequest();
            }
            try
            {
                _uow.Amenity.Update(amenity);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AmenityExists(id))
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

        // POST: api/Amenities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Amenity>> PostAmenity(Amenity amenity)
        {
            _uow.Amenity.Add(amenity);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetAmenity", new { id = amenity.Id }, amenity);
        }

        // DELETE: api/Amenities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(Guid id)
        {
            await _uow.Amenity.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> AmenityExists(Guid id)
        {
            return await _uow.Amenity.ExistsAsync(id);
        }
    }
}
