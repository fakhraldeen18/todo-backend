using System.ComponentModel.DataAnnotations;
using Harkh_backend.src.Enums;
using Harkh_backend.src.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harkh_backend.src.Entities;

[Index(nameof(Email), IsUnique = true)] // Index is to Search by email faster. 

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Role Role { get; set; } = Role.TeamMember;
    [Required, EmailAddress]
    public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public string? Position { get; set; }
    public string Password { get; set; }
    public string? Phone { get; set; }
    public string? ProfileImage { get; set; }
    public string? Nationality { get; set; }
    public DateTime CreatedAt { get; set; }
    // Navigation properties
    public List<Project> Project { get; set; }
    public List<Task> Tasks { get; set; }
    public List<Document> Documents { get; set; }
    public List<Experience> Experiences { get; set; }
    public List<Education> Educations { get; set; }
    public List<Invitation> Invitations { get; set; }
    public List<UserSkill> UserSkills { get; set; }
    public List<UserProject> UserProjects { get; set; }
}