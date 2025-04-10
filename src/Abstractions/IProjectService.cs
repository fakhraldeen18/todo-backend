using System.Collections;
using Harkh_backend.src.DTOs;

namespace Harkh_backend.src.Abstractions;

public interface IProjectService
{

    public Task<IEnumerable<ProjectReadDto>> FindAll();
    public Task<IEnumerable<ProjectReadDto>> FindAll(int limit, int offset);
    public Task<ProjectReadDto?> FindOne(Guid id);
    public Task<ProjectReadDto?> CreateOne(ProjectCreateDto newProject);
    public Task<bool> DeleteOne(Guid id);
    public Task<ProjectReadDto?> UpdateOne(Guid id, ProjectUpdateDto updatedProject);
    public Task<ProjectReadDto?> UpdateStatus(Guid id, ProjectUpdateStatusDto updatedProject);
    public Task<DocumentReadDto?> CreateDocument(DocumentCreateDto newDocument);
    public Task<IEnumerable<ProjectJoinMilestoneDto>?> GetMilestones(Guid id);
    public Task<IEnumerable?> GetDocuments(Guid id);
}
