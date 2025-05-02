using System.Collections;
using Harkh_backend.src.DTOs;

namespace Harkh_backend.src.Abstractions;

public interface IMilestoneService
{
    public Task<IEnumerable<MilestoneReadDto>> FindAll();
    public Task<MilestoneFullDataDto> FindAllFullData(Guid id);
    public Task<IEnumerable<MilestoneFullDataDto>?> FindAllProjectMilestonesData(Guid projectId);
    public Task<MilestoneReadDto?> FindOne(Guid id);
    public Task<MilestoneReadDto?> CreateOne(MilestoneCreateDto newMilestone);
    public Task<bool> DeleteOne(Guid id);
    public Task<bool> DeleteOneWithTasks(Guid id);
    public Task<MilestoneReadDto?> UpdateOne(Guid id, MilestoneUpdateDto updatedMilestone);
    // public MilestoneReadDto UpdateStatus(Guid id, MilestoneUpdateStatusDto updatedStatus);
    public Task<IEnumerable<MilestoneJoinTaskDto>?> GetTasks(Guid id);
    public Task<IEnumerable?> GetDocuments(Guid id);

}
