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
    public class HousesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public HousesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/Houses
        [HttpGet]
        public async Task<IEnumerable<House>> GetHouses()
        {
            return await _uow.House.GetAllAsync();
        }

        // GET: api/Houses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<House>> GetHouse(Guid id)
        {
            var house = await _uow.House.FirstOrDefaultAsync(id);

            if (house == null)
            {
                return NotFound();
            }

            return house;
        }

        // PUT: api/Houses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHouse(Guid id, House house)
        {
            try
            {
                _uow.House.Update(house);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await HouseExists(id))
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

        // POST: api/Houses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<House>> PostHouse(House house)
        {
          _uow.House.Add(house);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetHouse", new { id = house.Id }, house);
        }

        // DELETE: api/Houses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHouse(Guid id)
        {
            await _uow.House.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> HouseExists(Guid id)
        {
            return await _uow.House.ExistsAsync(id);
        }
    }
}
