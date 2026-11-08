namespace StaffVolunteerSystem.Models;

public enum AttendanceStatus { Present, Late, Absent }

public class AttendanceRecord
{
    public int Id { get; set; }
    public int PersonId { get; set; }
    public Person? Person { get; set; }

    public DateTime Date { get; set; } = DateTime.Today;
    public TimeSpan? CheckIn { get; set; }
    public TimeSpan? CheckOut { get; set; }
    public AttendanceStatus Status { get; set; }
}