#pragma warning disable CS1591
#nullable disable
using App.Contracts.Public;
using App.Public.DTO.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Areas.Authorized.Controllers;

[Area("Authorized")]
[Authorize(Roles = "admin,moderator")]
public class PersonsController : Controller
{
    private readonly ILogger<PersonsController> _logger;
    private readonly IAppPublic _public;

    public PersonsController(IAppPublic appPublic, ILogger<PersonsController> logger)
    {
        _public = appPublic;
        _logger = logger;
    }

    // GET: Admin/Persons
    public async Task<IActionResult> Index()
    {
        return View(await _public.Person.GetAllAsync());
    }

    // GET: Admin/Persons/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null) return NotFound();

        var person = await _public.Person.FirstOrDefaultAsync(id.Value);
        if (person == null) return NotFound();

        return View(person);
    }

    // GET: Admin/Persons/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Persons/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Name,Surname,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        Person person)
    {
        if (ModelState.IsValid)
        {
            person.Id = Guid.NewGuid();
            _public.Person.Add(person);
            await _public.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(person);
    }

    // GET: Admin/Persons/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null) return NotFound();

        var person = await _public.Person.FirstOrDefaultAsync(id.Value);
        if (person == null) return NotFound();
        return View(person);
    }

    // POST: Admin/Persons/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        [Bind("Name,Surname,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Id")]
        Person person)
    {
        if (id != person.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _public.Person.Update(person);
                await _public.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PersonExists(person.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(person);
    }

    // GET: Admin/Persons/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null) return NotFound();

        var person = await _public.Person.FirstOrDefaultAsync(id.Value);
        if (person == null) return NotFound();

        return View(person);
    }

    // POST: Admin/Persons/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _public.Person.RemoveAsync(id);
        await _public.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task<bool> PersonExists(Guid id)
    {
        return await _public.Person.ExistsAsync(id);
    }
}