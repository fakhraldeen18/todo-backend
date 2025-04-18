namespace Harkh_backend.src.Entities;

public class UserSkill
{
    public Guid UserId { get; set; } // foreign key
    public Guid SkillId { get; set; } // foreign key

    
    // Navigation properties
    public User User { get; set; }
    public Skill Skill { get; set; }
}
