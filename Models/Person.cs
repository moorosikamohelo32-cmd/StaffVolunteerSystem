using System.ComponentModel.DataAnnotations;

namespace StaffVolunteerSystem.Models;

public enum PersonType { Staff, Volunteer }

public class Person
{
    public int Id { get; set; }

    [Display(Name = "Full name")]
    [Required, StringLength(100)]
    public string FullName { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Phone]
    public string? Phone { get; set; }

    public PersonType Type { get; set; }

    public string? Department { get; set; }

     [Display(Name = "Join date")]
    public DateTime JoinDate { get; set; } = DateTime.Today;

    public bool IsActive { get; set; } = true;

    public List<AttendanceRecord> Attendance { get; set; } = new();
    public List<Assignment> Assignments { get; set; } = new();
}