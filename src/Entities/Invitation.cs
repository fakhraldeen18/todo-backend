
namespace Harkh_backend.src.Entities;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? ProjectId { get; set; }
    public string ToEmail { get; set; }
    public string Name { get; set; }
    public string ProjectName { get; set; }
    public string ProjectLink { get; set; }
    public string YourName { get; set; }
    public string YourEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
