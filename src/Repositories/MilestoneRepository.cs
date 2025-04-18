using System.Data;
using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Databases;
using Harkh_backend.src.Entities;
using Harkh_backend.src.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Harkh_backend.src.Repositories;

public class MilestoneRepository : IMilestoneRepository
{

    private readonly DbSet<Milestone> _milestones;
    private readonly DatabaseContext _databaseContext;
    private readonly DbSet<Entities.Task> _tasks;

    public MilestoneRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _milestones = _databaseContext.Milestones;
        _tasks = _databaseContext.Tasks;
    }

    public async Task<Milestone> CreateOne(Milestone newMilestone)
    {
        await _milestones.AddAsync(newMilestone);
        return newMilestone;
    }

    public Milestone? DeleteOne(Milestone milestone)
    {
        _milestones.Remove(milestone);
        return milestone;
    }

    public async Task<IEnumerable<Milestone>> FindAll()
    {
        return await _milestones.ToListAsync();
    }

    public async Task<Milestone?> FindAllFullData(Guid id)
    {
        return await _milestones
        .Include(m => m.Tasks)
        .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Milestone?> FindOne(Guid? id)
    {
        return await _milestones.FirstOrDefaultAsync(m => m.Id == id);
    }

    public Milestone UpdateOne(Milestone updatedMilestone)
    {
        _milestones.Update(updatedMilestone);
        return updatedMilestone;
    }
    public async Task<Milestone?> UpdateProgress(Guid? id)
    {
        Milestone? milestone = await _milestones.FirstOrDefaultAsync(m => m.Id == id);
        if (milestone == null) return null;
        var tasks = _tasks;
        var tasksWithMilestone = tasks.Where(task => task.MilestoneId == id);
        var numberOfTasks = tasks.Where(task => task.MilestoneId == id).Count();
        Console.WriteLine($"tasks {numberOfTasks}");
        double progress = 100;
        double totalProgress = 0;
        double wightOfTasks = progress / numberOfTasks;
        Console.WriteLine($"wightOfTasks: {Math.Round(wightOfTasks, 2)}");
        foreach (var task in tasksWithMilestone)
        {
            double taskProgress = (double)(task.Progress * wightOfTasks) / 100;
            Console.WriteLine($"progress: {Math.Round(taskProgress, 2)}%");
            totalProgress += Math.Round(taskProgress, 2);
        }

        Console.WriteLine($"progressOfMilestone: {totalProgress}%");
        milestone.Progress = (int)Math.Round(totalProgress);
        milestone.UpdateAt = DateTime.Now;
        _milestones.Update(milestone);
        return milestone;
    }
}
