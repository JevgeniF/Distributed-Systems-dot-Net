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
    public class FixedServicesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public FixedServicesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/FixedServices
        [HttpGet]
        public async Task<IEnumerable<FixedService>> GetFixedServices()
        {
            return await _uow.FixedService.GetAllAsync();
        }

        // GET: api/FixedServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FixedService>> GetFixedService(Guid id)
        {

            var fixedService = await _uow.FixedService.FirstOrDefaultAsync(id);

            if (fixedService == null)
            {
                return NotFound();
            }

            return fixedService;
        }

        // PUT: api/FixedServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFixedService(Guid id, FixedService fixedService)
        {
            if (id != fixedService.Id)
            {
                return BadRequest();
            }

            try
            {
                _uow.FixedService.Update(fixedService);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await FixedServiceExists(id))
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

        // POST: api/FixedServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FixedService>> PostFixedService(FixedService fixedService)
        {
          _uow.FixedService.Add(fixedService);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetFixedService", new { id = fixedService.Id }, fixedService);
        }

        // DELETE: api/FixedServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFixedService(Guid id)
        {
            await _uow.FixedService.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> FixedServiceExists(Guid id)
        {
            return await _uow.FixedService.ExistsAsync(id);
        }
    }
}
