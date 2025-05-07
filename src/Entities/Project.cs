using System.ComponentModel.DataAnnotations.Schema;
using Harkh_backend.src.Enums;

namespace Harkh_backend.src.Entities;

public class Project
{
    public Guid Id { get; set; }
    [Column("ManagerId")]
    public Guid UserId { get; set; } // foreign key
    public string? ManagerName { get; set; }
    public string Name { get; set; }
    public string? Avatar { get; set; }
    public string? Description { get; set; }
    public float? Progress { get; set; } = 0;
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Status { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateAt { get; set; }

    // Navigation properties
    public List<Milestone> Milestones { get; set; }
    public List<UserProject> UserProjects { get; set; }
}
