using System.Collections;
using Harkh_backend.src.DTOs;

namespace Harkh_backend.src.Abstractions;

public interface IUserProjectService
{

    public Task<IEnumerable<UsersProjectsReadDto>> FindAll();
    public Task<IEnumerable?> GetProjectUsers(Guid projectId);
    public Task<IEnumerable?> GetUserProjects(Guid userId, int limit, int offset, string? status = null);
    public Task<object?> FindOne(Guid projectId);
    public Task<bool> FindManager(Guid projectId, Guid? managerId);
    public Task<object?> InsightsCards(Guid projectId);
    public Task<object?> NumberOfProject(Guid userId);
    public Task<UsersProjectsReadDto?> CreateOne(UsersProjectsCreateDto newUserProject);
    public Task<bool> DeleteOne(Guid id, Guid projectId);
}
