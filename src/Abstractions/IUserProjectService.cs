using System.Collections;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;

namespace Harkh_backend.src.Abstractions;

public interface IUserProjectService
{

    public Task<IEnumerable<UsersProjectsReadDto>> FindAll();
    public Task<IEnumerable?> GetProjectUsers(Guid projectId);
    public Task<IEnumerable?> GetUserProjects(Guid userId,int limit, int offset);
    public Task<UsersProjectsReadDto?> CreateOne(UsersProjectsCreateDto newUserProject);
    public Task<bool> DeleteOne(Guid id);

}
