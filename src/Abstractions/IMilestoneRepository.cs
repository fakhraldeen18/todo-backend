using Harkh_backend.src.Entities;

namespace Harkh_backend.src.Abstractions;

public interface IMilestoneRepository
{
    public Task<IEnumerable<Milestone>> FindAll();
    public Task<Milestone?> FindAllFullData(Guid id);
    public Task<Milestone?> FindOne(Guid? id);
    public Task<Milestone> CreateOne(Milestone newMilestone);
    public Milestone UpdateOne(Milestone updatedMilestone);
    public Task<Milestone?> UpdateProgress(Guid? id);
    public Milestone? DeleteOne(Milestone milestone);

}
