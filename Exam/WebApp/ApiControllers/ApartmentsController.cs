using App.Contracts.DAL;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public ApartmentsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/Apartments
        [HttpGet]
        public async Task<IEnumerable<Apartment>> GetApartments()
        {
            return await _uow.Apartment.GetAllAsync();
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apartment>> GetApartment(Guid id)
        {
            var apartment = await _uow.Apartment.FirstOrDefaultAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            return apartment;
        }

        // PUT: api/Apartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(Guid id, Apartment apartment)
        {
            if (id != apartment.Id)
            {
                return BadRequest();
            }
            try
            {
                _uow.Apartment.Update(apartment);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApartmentExists(id))
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

        // POST: api/Apartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Apartment>> PostApartment(Apartment apartment)
        {
            _uow.Apartment.Add(apartment);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetApartment", new { id = apartment.Id }, apartment);
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(Guid id)
        {
            await _uow.Apartment.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ApartmentExists(Guid id)
        {
            return await _uow.Apartment.ExistsAsync(id);
        }
    }
}
