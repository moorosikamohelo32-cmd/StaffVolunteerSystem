using Microsoft.EntityFrameworkCore;
using StaffVolunteerSystem.Models;

namespace StaffVolunteerSystem.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Person> People => Set<Person>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<Responsibility> Responsibilities => Set<Responsibility>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
}