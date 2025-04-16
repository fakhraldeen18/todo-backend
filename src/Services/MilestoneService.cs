using System.Collections;
using AutoMapper;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class MilestoneService : IMilestoneService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IBaseRepository<Entities.Task> _taskRepository;
    private readonly IBaseRepository<Document> _documentRepository;


    public MilestoneService(IMilestoneRepository milestoneRepository, IProjectRepository projectRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _milestoneRepository = milestoneRepository;
        _projectRepository = projectRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _taskRepository = _unitOfWork.Tasks;
        _documentRepository = _unitOfWork.Documents;
    }

    public async Task<MilestoneReadDto?> CreateOne(MilestoneCreateDto newMilestone)
    {
        if (newMilestone == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            Milestone milestone = _mapper.Map<Milestone>(newMilestone);
            await _milestoneRepository.CreateOne(milestone);
            await _unitOfWork.Complete();
            if (milestone?.ProjectId == null) return _mapper.Map<MilestoneReadDto>(milestone);
            await _projectRepository.UpdateProgress(milestone.ProjectId);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<MilestoneReadDto>(milestone);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<bool> DeleteOne(Guid id)
    {
        Milestone? milestone = await _milestoneRepository.FindOne(id);
        if (milestone == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _milestoneRepository.DeleteOne(milestone);
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

    public async Task<IEnumerable<MilestoneReadDto>> FindAll()
    {
        IEnumerable<Milestone> milestones = await _milestoneRepository.FindAll();
        return _mapper.Map<IEnumerable<MilestoneReadDto>>(milestones);
    }

    public async Task<MilestoneFullDataDto> FindAllFullData(Guid id)
    {
        var milestones = await _milestoneRepository.FindAllFullData(id);
        MilestoneFullDataDto readMilestone = _mapper.Map<MilestoneFullDataDto>(milestones);
        return readMilestone;
    }

    public async Task<MilestoneReadDto?> FindOne(Guid id)
    {
        Milestone? milestone = await _milestoneRepository.FindOne(id);
        if (milestone == null) return null;
        return _mapper.Map<MilestoneReadDto>(milestone);
    }

    public async Task<MilestoneReadDto?> UpdateOne(Guid id, MilestoneUpdateDto updatedMilestone)
    {
        Milestone? milestone = await _milestoneRepository.FindOne(id);
        if (milestone == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            milestone.ProjectId = updatedMilestone.ProjectId;
            milestone.Name = updatedMilestone.Name;
            milestone.Progress = updatedMilestone.Progress;
            milestone.DueDate = updatedMilestone.DueDate;
            _milestoneRepository.UpdateOne(milestone);
            await _unitOfWork.Complete();
            if (milestone?.ProjectId == null) return _mapper.Map<MilestoneReadDto>(milestone);
            await _projectRepository.UpdateProgress(milestone.ProjectId);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<MilestoneReadDto>(milestone);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }


    public async Task<IEnumerable<MilestoneJoinTaskDto>?> GetTasks(Guid id)
    {
        var findMilestone = await _milestoneRepository.FindOne(id);
        if (findMilestone == null) return null;
        var milestones = await _milestoneRepository.FindAll();
        var tasks = await _taskRepository.FindAll();
        var milestoneTasks = from milestone in milestones
                             join task in tasks
                             on milestone.Id equals task.MilestoneId
                             where milestone.Id == id
                             select new MilestoneJoinTaskDto
                             {
                                 UserId = task.UserId,
                                 Title = task.Title,
                                 Description = task.Description,
                                 Progress = task.Progress,
                                 Status = task.Status,
                                 Priority = task.Priority,
                                 CreatedAt = task.CreatedAt,
                                 StartDate = task.StartDate,
                                 DueDate = task.DueDate
                             };
        return milestoneTasks;
    }


    public async Task<IEnumerable?> GetDocuments(Guid id)
    {
        var findMilestone = await _milestoneRepository.FindOne(id);
        if (findMilestone == null) return null;
        var milestones = await _milestoneRepository.FindAll();
        var documents = await _documentRepository.FindAll();
        var milestoneDocuments = from milestone in milestones
                                 join document in documents
                                 on milestone.Id equals document.FromId
                                 where milestone.Id == id
                                 select new
                                 {
                                     document.UserId,
                                     document.FileUrl
                                 };
        return milestoneDocuments;
    }
}
