namespace StaffVolunteerSystem.Models;

public class Responsibility
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }

    public List<Assignment> Assignments { get; set; } = new();
}