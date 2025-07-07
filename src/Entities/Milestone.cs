namespace Harkh_backend.src.Entities;
public class Milestone
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; } // Foreign key
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float? Progress { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }

    // Navigation properties
    public List<Task> Tasks { get; set; }
}
