using System.ComponentModel.DataAnnotations;

namespace Harkh_backend.src.DTOs;


public class UserSkillCreateDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid SkillId { get; set; }
}
public class UserSkillCreateRangeDto
{
    [Required]
    public Guid SkillId { get; set; }
}


public class UserSkillReadDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid SkillId { get; set; }
}
