namespace StaffVolunteerSystem.Models;

public enum AssignmentStatus { Pending, InProgress, Done }

public class Assignment
{
    public int Id { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }

    public int ResponsibilityId { get; set; }
    public Responsibility? Responsibility { get; set; }

    public DateTime AssignedOn { get; set; } = DateTime.Today;
    public AssignmentStatus Status { get; set; }
}