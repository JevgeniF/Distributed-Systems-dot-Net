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
    public class ApartInHousesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public ApartInHousesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/ApartInHouses
        [HttpGet]
        public async Task<IEnumerable<ApartInHouse>> GetApartInHouse()
        {
          return await _uow.ApartInHouse.GetAllAsync();
        }
        
        // GET: api/ApartInHouses
        [HttpGet ("house={id}")]
        public async Task<IEnumerable<ApartInHouse>> GetApartByHoue(Guid id)
        {
            return await _uow.ApartInHouse.GetAllByHouseId(id, true);
        }

        // GET: api/ApartInHouses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartInHouse>> GetApartInHouse(Guid id)
        {

            var apartInHouse = await _uow.ApartInHouse.FirstOrDefaultAsync(id);

            if (apartInHouse == null)
            {
                return NotFound();
            }

            return apartInHouse;
        }

        // PUT: api/ApartInHouses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartInHouse(Guid id, ApartInHouse apartInHouse)
        {
            if (id != apartInHouse.Id)
            {
                return BadRequest();
            }

            try
            {
                _uow.ApartInHouse.Update(apartInHouse);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ApartInHouseExists(id))
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

        // POST: api/ApartInHouses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApartInHouse>> PostApartInHouse(ApartInHouse apartInHouse)
        {
            _uow.ApartInHouse.Add(apartInHouse);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetApartInHouse", new { id = apartInHouse.Id }, apartInHouse);
        }

        // DELETE: api/ApartInHouses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartInHouse(Guid id)
        {
            await _uow.ApartInHouse.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> ApartInHouseExists(Guid id)
        {
            return await _uow.ApartInHouse.ExistsAsync(id);
        }
    }
}
