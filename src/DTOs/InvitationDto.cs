namespace Harkh_backend.src.DTOs;

public class InvitationDto
{
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public string ToEmail { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectLink { get; set; } = string.Empty;
    public string YourName { get; set; } = string.Empty;
    public string YourEmail { get; set; } = string.Empty;

}
public class InvitationReadeDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProjectId { get; set; }
    public string ToEmail { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string ProjectLink { get; set; } = string.Empty;
    public string YourName { get; set; } = string.Empty;
    public string YourEmail { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}
