using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StaffVolunteerSystem.Data;
using StaffVolunteerSystem.Models;

namespace StaffVolunteerSystem.Controllers;

public class ResponsibilitiesController : Controller
{
    private readonly AppDbContext _db;

    public ResponsibilitiesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var list = await _db.Responsibilities
            .Include(r => r.Assignments)
            .ThenInclude(a => a.Person)
            .OrderBy(r => r.DueDate)
            .ToListAsync();
        return View(list);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Responsibility responsibility)
    {
        if (string.IsNullOrWhiteSpace(responsibility.Title))
        {
            ModelState.AddModelError(nameof(Responsibility.Title), "Title is required.");
            return View(responsibility);
        }

        _db.Responsibilities.Add(responsibility);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Assign(int id)
    {
        var responsibility = await _db.Responsibilities.FindAsync(id);
        if (responsibility == null) return NotFound();

        ViewBag.People = await _db.People
            .Where(p => p.IsActive)
            .OrderBy(p => p.FullName)
            .Select(p => new SelectListItem(p.FullName + " (" + p.Type + ")", p.Id.ToString()))
            .ToListAsync();

        return View(responsibility);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int id, int personId)
    {
        var exists = await _db.Assignments
            .AnyAsync(a => a.ResponsibilityId == id && a.PersonId == personId);

        if (!exists)
        {
            _db.Assignments.Add(new Assignment { ResponsibilityId = id, PersonId = personId });
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(int id, AssignmentStatus status)
    {
        var assignment = await _db.Assignments.FindAsync(id);
        if (assignment != null)
        {
            assignment.Status = status;
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var responsibility = await _db.Responsibilities.FindAsync(id);
        if (responsibility != null)
        {
            _db.Responsibilities.Remove(responsibility);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}