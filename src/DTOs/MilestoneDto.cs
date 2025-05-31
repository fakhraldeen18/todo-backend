using Harkh_backend.src.Enums;

namespace Harkh_backend.src.DTOs;

public class MilestoneReadDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }
}




public class MilestoneFullDataDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }

    public int NumberOfTasks
    {
        get
        {
            return Tasks.Count;
        }
    }

    public ICollection<TaskReadDto> Tasks { get; set; }
           = new List<TaskReadDto>();
}

public class MilestoneCreateDto
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public float? Progress { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;
}
public class MilestoneUpdateDto
{
    public Guid ProjectId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public float? Progress { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? UpdateAt { get; set; } = DateTime.Now;
}
public class MilestoneUpdateProgressDto
{
    public float Progress { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
public class MilestoneJoinTaskDto
{
    public Guid UserId { get; set; } // foreign key
    public string Title { get; set; }
    public string? Description { get; set; }
    public float? Progress { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? DueDate { get; set; }
}
// public class MilestoneUpdateStatusDto
// {
//     public Status Status { get; set; }
//     public DateTime UpdateAt { get; set; } = DateTime.Now;
// }
