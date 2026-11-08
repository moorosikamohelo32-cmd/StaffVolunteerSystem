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

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Person person)
    {
        if (!ModelState.IsValid) return View(person);

        _db.People.Add(person);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}