using System.Collections;
using AutoMapper;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class ProjectService : IProjectService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectRepository _projectRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IBaseRepository<Document> _documentRepository;
    private readonly IBaseRepository<UserProject> _userProjectRepository;


    public ProjectService(IMapper mapper, IProjectRepository projectRepository, IUnitOfWork unitOfWork, IMilestoneRepository milestoneRepository)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _milestoneRepository = milestoneRepository;
        _documentRepository = _unitOfWork.Documents;
        _userProjectRepository = _unitOfWork.UserProjects;
    }

    public async Task<ProjectReadDto?> CreateOne(ProjectCreateDto newProject)
    {
        Project? project = _mapper.Map<Project>(newProject);
        if (project == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            var createdProject = await _projectRepository.CreateOne(project);
            var newProjectUser = new UserProject
            {
                ProjectId = createdProject.Id,
                UserId = createdProject.UserId
            };
            await _userProjectRepository.CreateOne(newProjectUser);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<ProjectReadDto>(project);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<bool> DeleteOne(Guid id)
    {
        Project? findProject = await _projectRepository.FindOne(id);
        if (findProject == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _projectRepository.DeleteOne(findProject);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return false;
        }
    }

    public async Task<IEnumerable<ProjectReadDto>> FindAll(int limit, int offset)
    {
        IEnumerable<Project> projects = await _projectRepository.FindAll(limit,offset);
        IEnumerable<ProjectReadDto> readProjects = _mapper.Map<IEnumerable<ProjectReadDto>>(projects);
        return readProjects;
    }
    
    public async Task<IEnumerable<ProjectReadDto>> FindAll()
    {
        IEnumerable<Project> projects = await _projectRepository.FindAll();
        IEnumerable<ProjectReadDto> readProjects = _mapper.Map<IEnumerable<ProjectReadDto>>(projects);
        return readProjects;
    }

    public async Task<ProjectReadDto?> FindOne(Guid id)
    {
        var findProject = await _projectRepository.FindOne(id);
        if (findProject == null) return null;
        return _mapper.Map<ProjectReadDto>(findProject);
    }

    public async Task<ProjectReadDto?> UpdateOne(Guid id, ProjectUpdateDto updatedProject)
    {
        Project? project = await _projectRepository.FindOne(id);
        if (project == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            project.UserId = updatedProject.UserId;
            project.Name = updatedProject.Name;
            project.Progress = updatedProject.Progress;
            project.Description = updatedProject.Description;
            project.StartDate = updatedProject.StartDate;
            project.EndDate = updatedProject.EndDate;
            project.Status = updatedProject.Status;
            project.UpdateAt = updatedProject.UpdateAt;
            _projectRepository.UpdateOne(project);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<ProjectReadDto>(project);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<ProjectReadDto?> UpdateStatus(Guid id, ProjectUpdateStatusDto updatedProject)
    {
        Project? project = await _projectRepository.FindOne(id);
        if (project == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            project.Status = updatedProject.Status;
            project.UpdateAt = updatedProject.UpdateAt;
            _projectRepository.UpdateOne(project);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<ProjectReadDto>(project);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<DocumentReadDto?> CreateDocument(DocumentCreateDto newDocument)
    {
        if (newDocument == null) return null;
        var document = _mapper.Map<Document>(newDocument);
        await _unitOfWork.BeginTransaction();
        try
        {
            await _documentRepository.CreateOne(document);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<DocumentReadDto>(document);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }


    public async Task<IEnumerable<ProjectJoinMilestoneDto>?> GetMilestones(Guid id)
    {
        Project? findProject = await _projectRepository.FindOne(id);
        if (findProject == null) return null;
        var projects = await _projectRepository.FindAll();
        var milestones = await _milestoneRepository.FindAll();
        var projectMilestones = from project in projects
                                join milestone in milestones
                                on project.Id equals milestone.ProjectId
                                where project.Id == id
                                select new ProjectJoinMilestoneDto
                                {
                                    Name = milestone.Name,
                                    Description = milestone.Description,
                                    Progress = milestone.Progress,
                                    StartDate = milestone.StartDate,
                                    DueDate = milestone.DueDate,
                                };
        return projectMilestones;

    }
    public async Task<IEnumerable?> GetDocuments(Guid id)
    {
        Project? findProject = await _projectRepository.FindOne(id);
        if (findProject == null) return null;
        var projects = await _projectRepository.FindAll();
        var documents = await _documentRepository.FindAll();
        var projectDocuments = from project in projects
                               join document in documents
                               on project.Id equals document.FromId
                               where project.Id == id
                               select new
                               {
                                   document.UserId,
                                   document.FileUrl
                               };
        return projectDocuments;

    }
}
