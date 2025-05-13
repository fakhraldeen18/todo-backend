using System.Collections;
using AutoMapper;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.DTOs;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;

namespace Harkh_backend.src.Services;

public class UserProjectService : IUserProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBaseRepository<UserProject> _userProjectRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBaseRepository<Entities.Task> _tasksRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IMilestoneRepository _milestoneRepository;
    private readonly IMapper _mapper;


    public UserProjectService(IUnitOfWork unitOfWork, IProjectRepository projectRepository, IMilestoneRepository milestoneRepository, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userProjectRepository = _unitOfWork.UserProjects;
        _userRepository = _unitOfWork.Users;
        _tasksRepository = _unitOfWork.Tasks;
        _projectRepository = projectRepository;
        _milestoneRepository = milestoneRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UsersProjectsReadDto>> FindAll()
    {
        var userProjects = await _userProjectRepository.FindAll();
        return _mapper.Map<IEnumerable<UsersProjectsReadDto>>(userProjects);
    }

    public async Task<IEnumerable?> GetProjectUsers(Guid projectId)
    {
        var findProject = await _projectRepository.FindOne(projectId);
        if (findProject == null) return null;
        var projects = await _projectRepository.FindAll();
        var users = await _userRepository.FindAll();
        var userProjects = await _userProjectRepository.FindAll();
        var projectUsers = from userProject in userProjects
                           join user in users
                           on userProject.UserId equals user.Id
                           join project in projects
                           on userProject.ProjectId equals project.Id
                           join manger in users
                           on project.UserId equals manger.Id
                           where project.Id == projectId
                           select new
                           {
                               managerName = manger.Name,
                               namOfProject = project.Name,
                               user.Id,
                               user.Name,
                               user.ProfileImage,
                               user.Email,
                               user.Position,
                               user.Role,
                           };
        return projectUsers;
    }

    public async Task<IEnumerable?> GetUserProjects(Guid userId, int limit, int offset)
    {
        var findUser = await _userRepository.FindOne(userId);
        if (findUser == null) return null;
        var projects = await _projectRepository.FindAll();
        var users = await _userRepository.FindAll();
        var userProjects = await _userProjectRepository.FindAll();
        var readUserProject = from userProject in userProjects
                              join user in users
                              on userProject.UserId equals user.Id
                              join project in projects
                              on userProject.ProjectId equals project.Id
                              where user.Id == userId
                              select new
                              {
                                  project.Id,
                                  project.Name,
                                  Manager = (from project in projects
                                             join manager in users
                                             on project.UserId equals manager.Id
                                             where project.Id == userId
                                             select new
                                             {
                                                 manager.Id,
                                                 manager.Name,
                                                 manager.ProfileImage,
                                             }).FirstOrDefault(),
                                  project.Avatar,
                                  project.Description,
                                  project.Progress,
                                  project.StartDate,
                                  project.DueDate,
                                  project.Status,
                                  project.CreateAt,
                                  project.UpdateAt,
                                  NumberOfProjects = !userProjects.Any(x => x.UserId == user.Id) ? 0 : userProjects.Count(x => x.UserId == user.Id),
                                  NumberOfMembers = userProjects
                                      .Where(x => x.ProjectId == project.Id)
                                      .Select(y => new
                                      {
                                          y.UserId
                                      }).ToList().Count
                              };
        if (limit == 0 && offset == 0) return readUserProject;
        return readUserProject.Skip(offset).Take(limit);

    }


    public async Task<object?> FindOne(Guid projectId)
    {
        var findProject = await _projectRepository.FindOne(projectId);
        if (findProject == null) return null;

        var projects = await _projectRepository.FindAll();
        var users = await _userRepository.FindAll();
        var userProjects = await _userProjectRepository.FindAll();
        var milestones = await _milestoneRepository.FindAll();
        var tasks = await _tasksRepository.FindAll();

        var projectDetails = from project in projects
                             join manager in users
                             on project.UserId equals manager.Id
                             join userProject in userProjects
                             on project.Id equals userProject.ProjectId
                             where project.Id == projectId
                             select new
                             {
                                 UserProjectId = userProject.Id,
                                 project.Id,
                                 project.Name,
                                 project.Description,
                                 project.Status,
                                 project.Avatar,
                                 Manager = (from project in projects
                                            join manager in users
                                            on project.UserId equals manager.Id
                                            where project.Id == projectId
                                            select new
                                            {
                                                manager.Id,
                                                manager.Name,
                                                manager.ProfileImage,
                                            }).FirstOrDefault(),
                                 Date = projects
                                      .Where(x => x.Id == project.Id)
                                      .Select(x => new
                                      {
                                          x.StartDate,
                                          x.DueDate,
                                      }).FirstOrDefault()
                             };
        var info = projectDetails.FirstOrDefault();
        if (info == null) return null;

        var findProjectDetail = from project in projects
                                join manager in users
                                on project.UserId equals manager.Id
                                join userProject in userProjects
                                on project.Id equals userProject.ProjectId
                                where project.Id == projectId
                                select new
                                {
                                    project.Id,
                                    info,
                                    members = (
                                              from userProject in userProjects
                                              join user in users
                                              on userProject.UserId equals user.Id
                                              where userProject.ProjectId == project.Id
                                              select new
                                              {
                                                  user.Id,
                                                  user.Name,
                                                  user.ProfileImage,
                                              }).ToList().Take(3),
                                    insightsCards = (from project in projects
                                                     where project.Id == projectId
                                                     let projectMilestones = milestones.Where(m => m.ProjectId == project.Id)
                                                     let milestoneTasks = projectMilestones
                                                         .SelectMany(m => tasks.Where(t => t.MilestoneId == m.Id))
                                                     select new
                                                     {
                                                         CompletedTasks = milestoneTasks.Count(t => t.Status == "done"),
                                                         RemainingTasks = milestoneTasks.Count(t => t.Status != "done"),
                                                         TotalTasks = milestoneTasks.Count()
                                                     }).DefaultIfEmpty(new
                                                     {
                                                         CompletedTasks = 0,
                                                         RemainingTasks = 0,
                                                         TotalTasks = 0
                                                     }).FirstOrDefault(),
                                    tasks = (
                                          from project in projects
                                          join milestone in milestones
                                          on project.Id equals milestone.ProjectId
                                          join task in tasks
                                          on milestone.Id equals task.MilestoneId
                                          where project.Id == projectId
                                          select new
                                          {
                                              task.Id,
                                              task.Title,
                                              assignee = users
                                                  .Where(x => x.Id == task.UserId)
                                                  .Select(x => new
                                                  {
                                                      x.Id,
                                                      x.Name,
                                                      x.ProfileImage,
                                                  }).FirstOrDefault(),
                                          })
                                };

        return findProjectDetail.FirstOrDefault();
    }

    public async Task<UsersProjectsReadDto?> CreateOne(UsersProjectsCreateDto newUserProject)
    {
        if (newUserProject.UserId == Guid.Empty || newUserProject.ProjectId == Guid.Empty) return null;
        var userProjects = await _userProjectRepository.FindAll();
        var findUserProject = userProjects
            .Where(x => x.UserId == newUserProject.UserId && x.ProjectId == newUserProject.ProjectId)
            .ToList().FirstOrDefault();
        if (findUserProject != null) return null;
        if (newUserProject == null) return null;
        await _unitOfWork.BeginTransaction();
        try
        {
            var createdUserProject = _mapper.Map<UserProject>(newUserProject);
            await _userProjectRepository.CreateOne(createdUserProject);
            await _unitOfWork.Complete();
            await _unitOfWork.CommitTransaction();
            return _mapper.Map<UsersProjectsReadDto>(createdUserProject);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransaction();
            return null;
        }
    }

    public async Task<bool> DeleteOne(Guid id, Guid projectId)
    {
        if (id == Guid.Empty || projectId == Guid.Empty) return false;
        var userProjects = await _userProjectRepository.FindAll();
        var findUserProject = userProjects
            .Where(x => x.UserId == id && x.ProjectId == projectId)
            .ToList().FirstOrDefault();
        if (findUserProject == null) return false;
        var result = await _userProjectRepository.FindOne(findUserProject.Id);
        if (result == null) return false;
        await _unitOfWork.BeginTransaction();
        try
        {
            _userProjectRepository.DeleteOne(result);
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

    public async Task<object?> InsightsCards(Guid projectId)
    {
        var findProject = await _projectRepository.FindOne(projectId);
        if (findProject == null) return null;

        var projects = await _projectRepository.FindAll();
        var milestones = await _milestoneRepository.FindAll();
        var tasks = await _tasksRepository.FindAll();

        var cards = from project in projects
                    join milestone in milestones
                    on project.Id equals milestone.ProjectId
                    join task in tasks
                 on milestone.Id equals task.MilestoneId
                    where project.Id == projectId
                    group task by 1 into g
                    select new
                    {
                        CompletedTasks = g.Count(t => t.Status == "done"),
                        RemainingTasks = g.Count(t => t.Status != "done"),
                        TotalTasks = g.Count()
                    };
        return cards.DefaultIfEmpty(new
        {
            CompletedTasks = 0,
            RemainingTasks = 0,
            TotalTasks = 0
        }).FirstOrDefault();
    }
    public async Task<object?> NumberOfProject(Guid userId)
    {
        var findUser = await _userRepository.FindOne(userId);
        if (findUser == null) return null;

        var projects = await _projectRepository.FindAll();
        var users = await _userRepository.FindAll();
        var userProjects = await _userProjectRepository.FindAll();

        var noProjects = from userProject in userProjects
                         join user in users
                         on userProject.UserId equals user.Id
                         join project in projects
                         on userProject.ProjectId equals project.Id
                         where user.Id == userId
                         select new
                         {
                             NumberOfProjects = userProjects.Count(x => x.UserId == user.Id) == 0 ? 0 : userProjects.Count(x => x.UserId == user.Id)
                         };
        return noProjects.DefaultIfEmpty(new
        {
            NumberOfProjects = 0
        }).FirstOrDefault();
    }

}