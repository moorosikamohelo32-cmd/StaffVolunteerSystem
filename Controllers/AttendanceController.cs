using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffVolunteerSystem.Data;
using StaffVolunteerSystem.Models;

namespace StaffVolunteerSystem.Controllers;

public class AttendanceController : Controller
{
    private readonly AppDbContext _db;

    public AttendanceController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(DateTime? date)
    {
        var day = (date ?? DateTime.Today).Date;

        var people = await _db.People
            .Where(p => p.IsActive)
            .OrderBy(p => p.FullName)
            .ToListAsync();

        var records = await _db.AttendanceRecords
            .Where(a => a.Date == day)
            .ToListAsync();

        var vm = new AttendancePageViewModel { Date = day };

        foreach (var p in people)
        {
            var rec = records.FirstOrDefault(r => r.PersonId == p.Id);
            vm.Rows.Add(new AttendanceRowViewModel
            {
                PersonId = p.Id,
                FullName = p.FullName,
                Type = p.Type,
                Status = rec?.Status ?? AttendanceStatus.Present,
                Recorded = rec != null
            });
        }

        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(AttendancePageViewModel model)
    {
        var day = model.Date.Date;

        var existing = await _db.AttendanceRecords
            .Where(a => a.Date == day)
            .ToListAsync();

        foreach (var row in model.Rows)
        {
            var rec = existing.FirstOrDefault(r => r.PersonId == row.PersonId);
            if (rec == null)
            {
                _db.AttendanceRecords.Add(new AttendanceRecord
                {
                    PersonId = row.PersonId,
                    Date = day,
                    Status = row.Status
                });
            }
            else
            {
                rec.Status = row.Status;
            }
        }

        await _db.SaveChangesAsync();
        TempData["Saved"] = $"Attendance saved for {day:dd MMM yyyy}.";
        return RedirectToAction(nameof(Index), new { date = day.ToString("yyyy-MM-dd") });
    }

    public async Task<IActionResult> History(int id)
    {
        var person = await _db.People
            .Include(p => p.Attendance)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (person == null) return NotFound();
        return View(person);
    }
}