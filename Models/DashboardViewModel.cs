namespace StaffVolunteerSystem.Models;

public class DashboardViewModel
{
    public int TotalPeople { get; set; }
    public int StaffCount { get; set; }
    public int VolunteerCount { get; set; }

    public int TodayPresent { get; set; }
    public int TodayLate { get; set; }
    public int TodayAbsent { get; set; }
    public int TodayRecorded => TodayPresent + TodayLate + TodayAbsent;
    public int AttendanceRate =>
        TodayRecorded == 0 ? 0 : (int)Math.Round((TodayPresent + TodayLate) * 100.0 / TodayRecorded);

    public int TasksPending { get; set; }
    public int TasksInProgress { get; set; }
    public int TasksDone { get; set; }

    public List<Person> RecentPeople { get; set; } = new();
}