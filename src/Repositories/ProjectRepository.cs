using System.Threading.Tasks;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Databases;
using Harkh_backend.src.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harkh_backend.src.Repositories;
public class ProjectRepository : IProjectRepository
{
    private readonly DbSet<Project> _projects;
    private readonly DatabaseContext _databaseContext;
    private readonly IMilestoneRepository _milestoneRepository;

    public ProjectRepository(DatabaseContext databaseContext, IMilestoneRepository milestoneRepository)
    {
        _databaseContext = databaseContext;
        _projects = _databaseContext.Projects;
        _milestoneRepository = milestoneRepository;
    }

    public async Task<Project> CreateOne(Project newProject)
    {
        await _projects.AddAsync(newProject);
        return newProject;
    }

    public Project DeleteOne(Project project)
    {
        _projects.Remove(project);
        return project;
    }

    public async Task<IEnumerable<Project>> FindAll()
    {
        return await _projects.ToListAsync();
    }

    public async Task<Project?> FindAllFullData(Guid id)
    {
        return await _projects
        .Include(p => p.Milestones)
        .ThenInclude(m => m.Tasks)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Project>> FindAll(int limit, int offset)
    {
        IEnumerable<Project> projects = await _projects.ToListAsync();
        if (limit == 0 & offset == 0) return projects;
        return projects.Skip(offset).Take(limit);
    }

    public async Task<Project?> FindOne(Guid id)
    {
        return await _projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public Project UpdateOne(Project updatedProject)
    {
        _projects.Update(updatedProject);
        return updatedProject;
    }

    public async Task<Project?> UpdateProgress(Guid id)
    {
        Project? project = await FindOne(id);
        if (project == null) return null;
        var milestones = await _milestoneRepository.FindAll();
        var milestonesWithProject = milestones.Where(milestone => milestone.ProjectId == id);
        var numberOfMilestones = milestonesWithProject.Count();
        Console.WriteLine($"milestones {numberOfMilestones}");
        double progress = 100;
        double totalProgress = 0;
        double wightOfMilestones = progress / numberOfMilestones;
        Console.WriteLine($"wightOfMilestones: {Math.Round(wightOfMilestones, 2)}");
        foreach (var milestone in milestonesWithProject)
        {
            double milestoneProgress = (double)(milestone.Progress * wightOfMilestones) / 100;
            Console.WriteLine($"progress: {Math.Round(milestoneProgress, 2)}%");
            totalProgress += Math.Round(milestoneProgress, 2);
        }
        Console.WriteLine($"progressOfProject: {totalProgress}%");
        project.Progress = (int)Math.Round(totalProgress);
        project.UpdateAt = DateTime.Now;
        _projects.Update(project);
        return project;
    }
}
