using System.ComponentModel.DataAnnotations.Schema;
using Harkh_backend.src.Enums;

namespace Harkh_backend.src.DTOs;

public class ProjectReadDto
{
    public Guid Id { get; set; }
    [Column("ManagerId")]
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }

}



public class ProjectFullDataDto
{
    public Guid Id { get; set; }
    [Column("ManagerId")]
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public DateTime CreateAt { get; set; }
    public DateTime UpdateAt { get; set; }

    public int NumberOfMilestones
    {
        get
        {
            return Milestones.Count;
        }
    }

    public ICollection<MilestoneFullDataDto> Milestones { get; set; }
           = new List<MilestoneFullDataDto>();
}





public class ProjectCreateDto
{
    [Column("ManagerId")]
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; } = 0;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime CreateAt { get; set; } = DateTime.Now;

}
public class ProjectUpdateDto
{
    [Column("ManagerId")]
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.Now;

}
public class ProjectUpdateStatusDto
{
    public ProjectStatus Status { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.Now;
}
public class ProjectUpdateProgressDto
{
    public float Progress { get; set; }
    public DateTime UpdateAt { get; set; } = DateTime.Now;

}
public class ProjectJoinMilestoneDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public float Progress { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime DueDate { get; set; }
}
