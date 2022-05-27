#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.ApiControllers;

/// <summary>
///     Controller for CRUD operations with  Person entities.
///     Person entities meant for storage of any required person name and surname.
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "admin,moderator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PersonsController : ControllerBase
{
    private readonly IAppPublic _public;

    /// <summary>
    ///     Constructor of PersonsController class
    /// </summary>
    /// <param name="appPublic">IAppPublic Interface of public layer</param>
    public PersonsController(IAppPublic appPublic)
    {
        _public = appPublic;
    }

    // GET: api/Persons
    /// <summary>
    ///     For admins and moderators only. Method returns list of all Person entities stored in API database.
    /// </summary>
    /// <returns>IEnumerable of Person entities.</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(IEnumerable<Person>), 200)]
    [HttpGet]
    public async Task<IEnumerable<Person>> GetPersons()
    {
        return await _public.Person.GetAllAsync();
    }

    // GET: api/Persons/5
    /// <summary>
    ///     For admins and moderators only. Method returns one exact Person entity found by it's id.
    /// </summary>
    /// <param name="id">Guid: Person entity Id</param>
    /// <returns>Person class entity</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Person), 200)]
    [ProducesResponseType(404)]
    [HttpGet("{id}")]
    public async Task<ActionResult<Person>> GetPerson(Guid id)
    {
        var person = await _public.Person.FirstOrDefaultAsync(id);

        if (person == null) return NotFound();

        return person;
    }

    // PUT: api/Persons/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    /// <summary>
    ///     For admins and moderators only. Method edits Person entity found in API database by it's id.
    /// </summary>
    /// <param name="id">Guid: Person entity id.</param>
    /// <param name="person">Updated Person entity to store under this id</param>
    /// <returns>Code 201 in case of success or Code 403 in case of wrong request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(201)]
    [ProducesResponseType(403)]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(Guid id, Person person)
    {
        if (id != person.Id) return BadRequest();

        try
        {
            _public.Person.Update(person);
            await _public.SaveChangesAsync();
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
    /// <summary>
    ///     For admins and moderators only. Method adds new Person entity to API database
    /// </summary>
    /// <param name="person">Person class entity to add</param>
    /// <returns>Added Person entity with it's id </returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(Person), 201)]
    [ProducesResponseType(403)]
    [HttpPost]
    public async Task<ActionResult<Person>> PostPerson(Person person)
    {
        person.Id = Guid.NewGuid();
        _public.Person.Add(person);
        await _public.SaveChangesAsync();

        return CreatedAtAction("GetPerson",
            new { id = person.Id, version = HttpContext.GetRequestedApiVersion()!.ToString() }, person);
    }

    // DELETE: api/Persons/5
    /// <summary>
    ///     For admins and moderators only. Deletes Person entity found by given id.
    /// </summary>
    /// <param name="id">Person entity id</param>
    /// <returns>Code 204 in case of success or code 404 in case of bad request</returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(Guid id)
    {
        await _public.Person.RemoveAsync(id);
        await _public.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> PersonExists(Guid id)
    {
        return await _public.Person.ExistsAsync(id);
    }
}