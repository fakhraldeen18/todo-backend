using System.Collections;
using AutoMapper;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class TaskService : ITaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<Entities.Task> _taskRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public TaskService(IMapper mapper, IMilestoneRepository milestoneRepository, IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _taskRepository = _unitOfWork.Tasks;
        _userRepository = _unitOfWork.Users;
        _milestoneRepository = milestoneRepository;
        _mapper = mapper;
        _projectRepository = projectRepository;
    }

    public async Task<TaskReadDto?> CreateOne(TaskCreteDto newTask)
    {
        Entities.Task readTask = _mapper.Map<Entities.Task>(newTask);
        if (readTask == null) return null;
        if (readTask.Status.ToLower() == "done") readTask.Progress = 100;
        await _unitOfWork.BeginTransaction();
        try
        {
            await _taskRepository.CreateOne(readTask);
            await _unitOfWork.Complete();
            if (readTask.MilestoneId == null)
            {
                await _unitOfWork.CommitTransaction();
                return _mapper.Map<TaskReadDto>(readTask);
            }

            Milestone? milestone = await _milestoneRepository.FindOne(readTask.MilestoneId);
            await _milestoneRepository.UpdateProgress(milestone?.Id);
            await _unitOfWork.Complete();
            Project? project = await _projectRepository.FindOne(milestone!.ProjectId);
            await _projectRepository.UpdateProgress(project!.Id);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<TaskReadDto>(readTask);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<bool> DeleteOne(Guid id)
    {
        Entities.Task? findTask = await _taskRepository.FindOne(id);
        if (findTask == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _taskRepository.DeleteOne(findTask);
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

    public async Task<IEnumerable<TaskReadDto>> FindAll()
    {
        IEnumerable<Entities.Task> tasks = await _taskRepository.FindAll();
        IEnumerable<TaskReadDto> readTasks = _mapper.Map<IEnumerable<TaskReadDto>>(tasks);
        return readTasks;
    }

    public async Task<TaskReadDto?> FindOne(Guid id)
    {
        Entities.Task? findTask = await _taskRepository.FindOne(id);
        if (findTask == null) return null;
        return _mapper.Map<TaskReadDto>(findTask); ;
    }

    public async Task<TaskReadDto?> UpdateOne(Guid id, TaskUpdateDto updatedTask)
    {
        Entities.Task? task = await _taskRepository.FindOne(id);
        if (task == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            task.UserId = updatedTask.UserId;
            task.AssigneeTo = updatedTask.AssigneeTo;
            task.MilestoneId = updatedTask.MilestoneId;
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.Status = updatedTask.Status;
            task.Priority = updatedTask.Priority;
            task.DueDate = updatedTask.DueDate;
            task.UpdateAt = updatedTask.UpdateAt;

            if (task.Status.ToLower() == "done") task.Progress = 100;
            _taskRepository.UpdateOne(task);
            await _unitOfWork.Complete();
            if (task.MilestoneId == null)
            {
                await _unitOfWork.CommitTransaction();
                return _mapper.Map<TaskReadDto>(task);
            }
            Milestone? milestone = await _milestoneRepository.FindOne(task.MilestoneId);
            await _milestoneRepository.UpdateProgress(milestone?.Id);
            await _unitOfWork.Complete();
            Project? project = await _projectRepository.FindOne(milestone!.ProjectId);
            await _projectRepository.UpdateProgress(project!.Id);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<TaskReadDto>(task);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<TaskReadDto?> UpdateStatus(Guid id, TaskUpdateStatusDto updatedStatus)
    {
        Entities.Task? task = await _taskRepository.FindOne(id);
        if (task == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            task.Status = updatedStatus.Status;
            if (task.Status.ToLower() == "done") task.Progress = 100;
            _taskRepository.UpdateOne(task);
            await _unitOfWork.Complete();
            if (task.MilestoneId == null)
            {
                await _unitOfWork.CommitTransaction();
                return _mapper.Map<TaskReadDto>(task);
            }

            Milestone? milestone = await _milestoneRepository.FindOne(task.MilestoneId);
            await _milestoneRepository.UpdateProgress(milestone?.Id);
            await _unitOfWork.Complete();
            Project? project = await _projectRepository.FindOne(milestone!.ProjectId);
            await _projectRepository.UpdateProgress(project!.Id);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<TaskReadDto>(task);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
    public async Task<TaskReadDto?> UpdatePriority(Guid id, TaskUpdatePriorityDto updatedProgress)
    {
        Entities.Task? task = await _taskRepository.FindOne(id);
        if (task == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            task.Priority = updatedProgress.Priority;
            _taskRepository.UpdateOne(task);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<TaskReadDto>(task);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }
    public async Task<TaskReadDto?> UpdateMilestone(Guid id, TaskUpdateMilestoneDto updatedMilestone)
    {
        Entities.Task? task = await _taskRepository.FindOne(id);
        if (task == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            task.MilestoneId = updatedMilestone.MilestoneId;
            _taskRepository.UpdateOne(task);
            await _unitOfWork.Complete();
            if (task.MilestoneId == null)
            {
                await _unitOfWork.CommitTransaction();
                return _mapper.Map<TaskReadDto>(task);
            }

            Milestone? milestone = await _milestoneRepository.FindOne(task.MilestoneId);
            await _milestoneRepository.UpdateProgress(milestone?.Id);
            await _unitOfWork.Complete();
            Project? project = await _projectRepository.FindOne(milestone!.ProjectId);
            await _projectRepository.UpdateProgress(project!.Id);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<TaskReadDto>(task);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<IEnumerable?> GetUserTasks(Guid userId)
    {

        var findUser = await _userRepository.FindOne(userId);
        if (findUser == null) return null;
        var tasks = await _taskRepository.FindAll();
        var userTask = tasks.Where(x => x.UserId == userId);
        return userTask;
    }
    public async Task<IEnumerable?> GetUserTasksFullData(Guid userId)
    {

        var findUser = await _userRepository.FindOne(userId);
        if (findUser == null) return null;

        var tasks = await _taskRepository.FindAll();
        var users = await _userRepository.FindAll();

        var userTask = from task in tasks
                       join user in users on task.UserId equals user.Id
                       where task.UserId == userId
                       select new
                       {
                           task.Id,
                           task.Title,
                           CreateBy = new
                           {
                               user.Id,
                               user.Name,
                               user.ProfileImage
                           },
                           Assignee = (from nestedTasks in tasks
                                      join nestedUser in users on task.UserId equals user.Id
                                      where task.AssigneeTo == nestedUser.Id
                                      select new
                                      {
                                          nestedUser.Id,
                                          nestedUser.Name,
                                          user.ProfileImage
                                      }).FirstOrDefault(),
                           //    Assignee = new
                           //    {
                           //        user.Id,
                           //        user.Name,
                           //        user.ProfileImage
                           //    },
                           task.Description,
                           task.Priority,
                           task.Status,
                           task.MilestoneId,
                           task.StartDate,
                           task.DueDate
                       };
        return userTask;
    }
}
