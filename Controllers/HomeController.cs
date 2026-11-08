using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffVolunteerSystem.Data;
using StaffVolunteerSystem.Models;

namespace StaffVolunteerSystem.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;

        var vm = new DashboardViewModel
        {
            TotalPeople = await _db.People.CountAsync(p => p.IsActive),
            StaffCount = await _db.People.CountAsync(p => p.IsActive && p.Type == PersonType.Staff),
            VolunteerCount = await _db.People.CountAsync(p => p.IsActive && p.Type == PersonType.Volunteer),

            TodayPresent = await _db.AttendanceRecords.CountAsync(a => a.Date == today && a.Status == AttendanceStatus.Present),
            TodayLate = await _db.AttendanceRecords.CountAsync(a => a.Date == today && a.Status == AttendanceStatus.Late),
            TodayAbsent = await _db.AttendanceRecords.CountAsync(a => a.Date == today && a.Status == AttendanceStatus.Absent),

            TasksPending = await _db.Assignments.CountAsync(a => a.Status == AssignmentStatus.Pending),
            TasksInProgress = await _db.Assignments.CountAsync(a => a.Status == AssignmentStatus.InProgress),
            TasksDone = await _db.Assignments.CountAsync(a => a.Status == AssignmentStatus.Done),

            RecentPeople = await _db.People
                .OrderByDescending(p => p.Id)
                .Take(5)
                .ToListAsync()
        };

        return View(vm);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}