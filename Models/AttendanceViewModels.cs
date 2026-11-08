namespace StaffVolunteerSystem.Models;

public class AttendanceRowViewModel
{
    public int PersonId { get; set; }
    public string FullName { get; set; } = "";
    public PersonType Type { get; set; }
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public bool Recorded { get; set; }
}

public class AttendancePageViewModel
{
    public DateTime Date { get; set; } = DateTime.Today;
    public List<AttendanceRowViewModel> Rows { get; set; } = new();
}