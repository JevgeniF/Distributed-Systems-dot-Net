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
    public class MonthlyServicesController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public MonthlyServicesController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/MonthlyServices
        [HttpGet]
        public async Task<IEnumerable<App.DAL.DTO.MonthlyService>> GetMonthlyServices()
        {
            return await _uow.MonthlyService.GetAllAsync();
        }

        // GET: api/MonthlyServices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MonthlyService>> GetMonthlyService(Guid id)
        {
            var monthlyService = await _uow.MonthlyService.FirstOrDefaultAsync(id);

            if (monthlyService == null)
            {
                return NotFound();
            }

            return monthlyService;
        }

        // PUT: api/MonthlyServices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMonthlyService(Guid id, MonthlyService monthlyService)
        {
            try
            {
                _uow.MonthlyService.Update(monthlyService);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MonthlyServiceExists(id))
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

        // POST: api/MonthlyServices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MonthlyService>> PostMonthlyService(MonthlyService monthlyService)
        {
            _uow.MonthlyService.Add(monthlyService);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetMonthlyService", new { id = monthlyService.Id }, monthlyService);
        }

        // DELETE: api/MonthlyServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonthlyService(Guid id)
        {

            await _uow.MonthlyService.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> MonthlyServiceExists(Guid id)
        {
            return await _uow.MonthlyService.ExistsAsync(id);
        }
    }
}
