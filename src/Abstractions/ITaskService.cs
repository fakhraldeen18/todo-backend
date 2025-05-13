using System.Collections;
using Harkh_backend.src.DTOs;

namespace Harkh_backend.src.Abstractions;

public interface ITaskService
{
    public Task<IEnumerable<TaskReadDto>> FindAll();
    public Task<TaskReadDto?> FindOne(Guid id);
    public Task<IEnumerable?> GetUserTasks(Guid userId);
    public Task<TaskReadDto?> CreateOne(TaskCreteDto newTask);
    public Task<bool> DeleteOne(Guid id);
    public Task<TaskReadDto?> UpdateOne(Guid id, TaskUpdateDto updatedTask);
    public Task<TaskReadDto?> UpdateStatus(Guid id, TaskUpdateStatusDto updatedStatus);
    public Task<TaskReadDto?> UpdatePriority(Guid id, TaskUpdatePriorityDto updatedProgress);
    public Task<TaskReadDto?> UpdateMilestone(Guid id, TaskUpdateMilestoneDto updatedMilestone);
}
