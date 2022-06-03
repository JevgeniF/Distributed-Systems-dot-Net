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
    public class RentMonthlyServicesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public RentMonthlyServicesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/RentMonthlyServices
        [HttpGet]
        public async Task<IEnumerable<RentMonthlyService>> GetRentMonthlyServices()
        {
            return await _uow.RentMonthlyService.GetAllAsync();
        }
        
        // GET: api/RentMonthlyServices
        [HttpGet ("rent={id}")]
        public async Task<IEnumerable<RentMonthlyService>> GetRentMonthlyServicesByRent(Guid id)
        {
            return await _uow.RentMonthlyService.GetAllByRentId(id, true);
        }

        // GET: api/RentMonthlyServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentMonthlyService>> GetRentMonthlyService(Guid id)
        {
            var rentMonthlyService = await _uow.RentMonthlyService.FirstOrDefaultAsync(id);

            if (rentMonthlyService == null)
            {
                return NotFound();
            }

            return rentMonthlyService;
        }

        // PUT: api/RentMonthlyServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentMonthlyService(Guid id, RentMonthlyService rentMonthlyService)
        {
            try
            {
                _uow.RentMonthlyService.Update(rentMonthlyService);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RentMonthlyServiceExists(id))
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

        // POST: api/RentMonthlyServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RentMonthlyService>> PostRentMonthlyService(RentMonthlyService rentMonthlyService)
        {
          _uow.RentMonthlyService.Add(rentMonthlyService);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetRentMonthlyService", new { id = rentMonthlyService.Id }, rentMonthlyService);
        }

        // DELETE: api/RentMonthlyServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentMonthlyService(Guid id)
        {
            await _uow.RentMonthlyService.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> RentMonthlyServiceExists(Guid id)
        {
            return await _uow.RentMonthlyService.ExistsAsync(id);
        }
    }
}
