using App.Contracts.DAL;
using App.DAL.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartRentsController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public ApartRentsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/ApartRents
        [HttpGet]
        public async Task<IEnumerable<App.DAL.DTO.ApartRent>> GetApartRents()
        {
            return await _uow.ApartRent.GetAllAsync();
        }
        
        // GET: api/ApartRents/apart
        [HttpGet("apart={id}")]
        public async Task<IEnumerable<App.DAL.DTO.ApartRent>> GetApartRentsByApart(Guid id)
        {
            return await _uow.ApartRent.GetAllByApartId(id, true);
        }
        
        // GET: api/ApartRents/person
        [HttpGet("person={id}")]
        public async Task<IEnumerable<App.DAL.DTO.ApartRent>> GetApartRentsByPerson(Guid id)
        {
            return await _uow.ApartRent.GetAllByPersonId(id, true);
        }

        // GET: api/ApartRents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartRent>> GetApartRent(Guid id)
        {
            var apartRent = await _uow.ApartRent.FirstOrDefaultAsync(id);

            if (apartRent == null)
            {
                return NotFound();
            }

            return apartRent;
        }

        // PUT: api/ApartRents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartRent(Guid id, ApartRent apartRent)
        {
            if (id != apartRent.Id)
            {
                return BadRequest();
            }
            try
            {
                _uow.ApartRent.Update(apartRent);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApartRentExists(id))
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

        // POST: api/ApartRents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApartRent>> PostApartRent(ApartRent apartRent)
        {
            _uow.ApartRent.Add(apartRent);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetApartRent", new { id = apartRent.Id }, apartRent);
        }

        // DELETE: api/ApartRents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartRent(Guid id)
        {

            await _uow.ApartRent.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ApartRentExists(Guid id)
        {
            return await _uow.ApartRent.ExistsAsync(id);
        }
    }
}
