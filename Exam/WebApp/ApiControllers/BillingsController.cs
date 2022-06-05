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
    public class BillingsController : ControllerBase
    {
        private readonly IAppUOW _uow;

        public BillingsController(IAppUOW uow)
        {
            _uow = uow;
        }

        // GET: api/Billings
        [HttpGet]
        public async Task<IEnumerable<Billing>> GetBillings()
        {
            return await _uow.Billing.GetAllAsync();
        }
        
        // GET: api/Billings
        [HttpGet ("person={id}")]
        public async Task<IEnumerable<Billing>> GetBillingsByPerson(Guid id)
        {
            return await _uow.Billing.GetAllByPersonId(id, true);
        }
        
        // GET: api/Billings
        [HttpGet ("rent={id}")]
        public async Task<IEnumerable<Billing>> GetBillingsByRentId(Guid id)
        {
            return await _uow.Billing.GetAllByRentId(id, true);
        }

        // GET: api/Billings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Billing>> GetBilling(Guid id)
        {
            var billing = await _uow.Billing.FirstOrDefaultAsync(id);

            if (billing == null)
            {
                return NotFound();
            }

            return billing;
        }

        // PUT: api/Billings/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBilling(Guid id, Billing billing)
        {
            if (id != billing.Id)
            {
                return BadRequest();
            }
            try
            {
                _uow.Billing.Update(billing);
                await _uow.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BillingExists(id))
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

        // POST: api/Billings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Billing>> PostBilling(Billing billing)
        {
            _uow.Billing.Add(billing);
            await _uow.SaveChangesAsync();

            return CreatedAtAction("GetBilling", new { id = billing.Id }, billing);
        }

        // DELETE: api/Billings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBilling(Guid id)
        {
            await _uow.Billing.RemoveAsync(id);
            await _uow.SaveChangesAsync();

            return NoContent();
        }

        private async Task<bool> BillingExists(Guid id)
        {
            return await _uow.Billing.ExistsAsync(id);
        }
    }
}
