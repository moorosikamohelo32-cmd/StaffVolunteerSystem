using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffVolunteerSystem.Data;
using StaffVolunteerSystem.Models;

namespace StaffVolunteerSystem.Controllers;

public class PeopleController : Controller
{
    private readonly AppDbContext _db;

    public PeopleController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search, PersonType? type)
    {
        var query = _db.People.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(p => p.FullName.Contains(search) || p.Email.Contains(search));

        if (type.HasValue)
            query = query.Where(p => p.Type == type.Value);

        ViewData["Search"] = search;
        return View(await query.OrderBy(p => p.FullName).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var person = await _db.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null) return NotFound();
        return View(person);
    }

    public IActionResult Create() => View(new Person());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Person person)
    {
        if (!ModelState.IsValid) return View(person);

        _db.People.Add(person);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var person = await _db.People.FindAsync(id);
        if (person == null) return NotFound();
        return View(person);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Person person)
    {
        if (id != person.Id) return NotFound();
        if (!ModelState.IsValid) return View(person);

        _db.Update(person);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var person = await _db.People.FirstOrDefaultAsync(p => p.Id == id);
        if (person == null) return NotFound();
        return View(person);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var person = await _db.People.FindAsync(id);
        if (person != null)
        {
            _db.People.Remove(person);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}