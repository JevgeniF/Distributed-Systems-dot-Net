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
    public class RentFixedServicesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public RentFixedServicesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/RentFixedServices
        [HttpGet]
        public async Task<IEnumerable<App.DAL.DTO.RentFixedService>> GetRentFixedServices()
        {
            return await _uow.RentFixedService.GetAllAsync();
        }
        
        // GET: api/RentFixedServices
        [HttpGet ("rent={id}")]
        public async Task<IEnumerable<App.DAL.DTO.RentFixedService>> GetRentFixedServicesByRent(Guid id)
        {
            return await _uow.RentFixedService.GetAllByRentId(id, true);
        }

        // GET: api/RentFixedServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentFixedService>> GetRentFixedService(Guid id)
        {
            var rentFixedService = await _uow.RentFixedService.FirstOrDefaultAsync(id);

            if (rentFixedService == null)
            {
                return NotFound();
            }

            return rentFixedService;
        }

        // PUT: api/RentFixedServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentFixedService(Guid id, RentFixedService rentFixedService)
        {
            try
            {
                _uow.RentFixedService.Update(rentFixedService);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await RentFixedServiceExists(id))
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

        // POST: api/RentFixedServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RentFixedService>> PostRentFixedService(RentFixedService rentFixedService)
        {
          _uow.RentFixedService.Add(rentFixedService);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetRentFixedService", new { id = rentFixedService.Id }, rentFixedService);
        }

        // DELETE: api/RentFixedServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRentFixedService(Guid id)
        {
         
            await _uow.RentFixedService.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> RentFixedServiceExists(Guid id)
        {
            return await _uow.RentFixedService.ExistsAsync(id);
        }
    }
}
