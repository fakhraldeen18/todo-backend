using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Databases;
using Harkh_backend.src.Entities;
using Harkh_backend.src.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Harkh_backend.src.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DatabaseContext _databaseContext;
    public IBaseRepository<User> Users { get; private set; }
    public IBaseRepository<Entities.Task> Tasks { get; private set; }
    public IBaseRepository<Document> Documents { get; private set; }
    public IBaseRepository<Skill> Skills { get; private set; }
    public IBaseRepository<Experience> Experiences { get; private set; }
    public IBaseRepository<Education> Educations { get; private set; }
    public IBaseRepository<UserSkill> UserSkills { get; private set; }
    public IBaseRepository<UserProject> UserProjects { get; private set; }
    public IBaseRepository<Invitation> Invitations { get; private set; }

    public UnitOfWork(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        Users = new BaseRepository<User>(_databaseContext);
        Tasks = new BaseRepository<Entities.Task>(_databaseContext);
        Documents = new BaseRepository<Document>(_databaseContext);
        Experiences = new BaseRepository<Experience>(_databaseContext);
        Educations = new BaseRepository<Education>(_databaseContext);
        Skills = new BaseRepository<Skill>(_databaseContext);
        UserSkills = new BaseRepository<UserSkill>(_databaseContext);
        UserProjects = new BaseRepository<UserProject>(_databaseContext);
        Invitations = new BaseRepository<Invitation>(_databaseContext);
    }

    public async Task<bool> Complete()
    {
        return await _databaseContext.SaveChangesAsync() >= 0;
    }

    public void Dispose()
    {
        _databaseContext.Dispose();
    }

    public async Task<IDbContextTransaction> BeginTransaction()
    {
        return await _databaseContext.Database.BeginTransactionAsync();
    }

    public async System.Threading.Tasks.Task CommitTransaction()
    {
        await _databaseContext.Database.CommitTransactionAsync();
    }

    public async System.Threading.Tasks.Task RollbackTransaction()
    {
        await _databaseContext.Database.RollbackTransactionAsync();
    }
}
