namespace Harkh_backend.src.Entities;

public class UserProject
{
    public Guid UserId { get; set; } // foreign key
    public Guid ProjectId { get; set; } // foreign key


    // Navigation properties
    public User User { get; set; }
    public Project Project { get; set; }
}
