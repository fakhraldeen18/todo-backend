using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Harkh_backend.src.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    public IBaseRepository<User> Users { get; }
    public IBaseRepository<Document> Documents { get; }
    public IBaseRepository<Skill> Skills { get; }
    public IBaseRepository<Experience> Experiences { get; }
    public IBaseRepository<Education> Educations { get; }
    public IBaseRepository<Entities.Task> Tasks { get; }
    public IBaseRepository<UserSkill> UserSkills { get; }
    public IBaseRepository<UserProject> UserProjects { get; }
    public IBaseRepository<Invitation> Invitations { get; }


    public Task<IDbContextTransaction> BeginTransaction();
    public System.Threading.Tasks.Task CommitTransaction();
    public System.Threading.Tasks.Task RollbackTransaction();
    public Task<bool> Complete();
}