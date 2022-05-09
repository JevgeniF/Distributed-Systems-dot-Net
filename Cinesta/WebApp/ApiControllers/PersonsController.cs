#nullable disable
using App.Contracts.DAL;
using App.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

[Route("api/[controller]")]
[Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IAppUOW _uow;

    public PersonsController(IAppUOW uow)
    {
        _uow = uow;
    }

    // GET: api/Persons
    [HttpGet]
    public async Task<IEnumerable<Person>> GetPersons()
    {
        return await _uow.Person.GetAllAsync();
    }

    // GET: api/Persons/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(Guid id)
    {
        var person = await _uow.Person.FirstOrDefaultAsync(id);

        if (person == null) return NotFound();

        return person;
    }

    // PUT: api/Persons/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(Guid id, Person person)
    {
        if (id != person.Id) return BadRequest();

        try
        {
            _uow.Person.Update(person);
            await _uow.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await PersonExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Persons
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(Person person)
    {
        _uow.Person.Add(person);
        await _uow.SaveChangesAsync();

        return CreatedAtAction("GetPerson", new {id = person.Id}, person);
    }

    // DELETE: api/Persons/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await _uow.Person.RemoveAsync(id);
        await _uow.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> PersonExists(Guid id)
    {
        return await _uow.Person.ExistsAsync(id);
    }
}