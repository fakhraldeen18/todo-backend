using Harkh_backend.src.Entities;
using Microsoft.EntityFrameworkCore;
using Harkh_backend.src.Enums;
namespace Harkh_backend.src.Databases;
public class DatabaseContext : DbContext // DbContext is built in class to give me access to database (gateway to database)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Entities.Task> Tasks { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Milestone> Milestones { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<Experience> Experiences { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<UserSkill> UserSkills { get; set; }
    public DbSet<UserProject> UserProjects { get; set; }
    public DbSet<Invitation> Invitations { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Status>(); // add the type Status
        modelBuilder.HasPostgresEnum<Priority>(); // add the type Priority
        modelBuilder.HasPostgresEnum<Role>(); // add the type Role
        modelBuilder.HasPostgresEnum<ProjectStatus>(); // add the type Project status

        
    }
}