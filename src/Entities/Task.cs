using System.ComponentModel.DataAnnotations;
using Harkh_backend.src.Enums;

namespace Harkh_backend.src.Entities;
public class Task
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // foreign key
    public Guid? MilestoneId { get; set; } // foreign key
    public string Title { get; set; }
    public string? Description { get; set; }
    public float? Progress { get; set; } = 0;
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? UpdateAt { get; set; }

    // Navigation property
    public Milestone? Milestone { get; set; }
}
