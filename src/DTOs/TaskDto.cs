using System.ComponentModel.DataAnnotations;
using Harkh_backend.src.Enums;

namespace Harkh_backend.src.DTOs;

public class TaskReadDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? AssigneeTo { get; set; }
    public Guid MilestoneId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime UpdateAt { get; set; }
}
public class TaskCreteDto
{
    public Guid UserId { get; set; }
    public Guid? MilestoneId { get; set; }
    public Guid? AssigneeTo { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public string Status { get; set; }
    public string Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
public class TaskUpdateDto
{
    public Guid UserId { get; set; }
    public Guid? AssigneeTo { get; set; }
    public Guid? MilestoneId { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    [Required]
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
public class TaskUpdateStatusDto
{
    [Required]
    public string Status { get; set; }
    [Required]
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
public class TaskUpdatePriorityDto
{
    [Required]
    public string Priority { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
public class TaskUpdateMilestoneDto
{
    [Required]
    public Guid? MilestoneId { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
