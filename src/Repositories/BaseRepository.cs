using Harkh_backend.src.Abstractions;
using Harkh_backend.src.Databases;
using Microsoft.EntityFrameworkCore;

namespace Harkh_backend.src.Repositories;
public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly DatabaseContext _databaseContext;
    private readonly DbSet<T> _data;

    public BaseRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        _data = _databaseContext.Set<T>();
    }

    public async Task<T> CreateOne(T newObj)
    {
        await _data.AddAsync(newObj);
        return newObj;
    }

    public async Task<IEnumerable<T>> CreateRange(IEnumerable<T> newObjs)
    {
        await  _data.AddRangeAsync(newObjs);
        return newObjs;
    }

    public T? DeleteOne(T Obj)
    {
     _data.Remove(Obj);
        return Obj;
    }

    public IEnumerable<T> DeleteRange(IEnumerable<T> objs)
    {
        _data.RemoveRange(objs);
        return objs;
    }

    public async Task<IEnumerable<T>> FindAll()
    {
        return await _data.ToListAsync();
    }

    public async Task<T?> FindOne(Guid id)
    {
        T? obj = await _data.FindAsync(id);
        return obj;
    }

    public T UpdateOne(T updatedObj)
    {
     _data.Update(updatedObj);
        return updatedObj;
    }

    // public T? Find(Expression<Func<T, bool>> match, string[] includes = null)
    // {
    //     IQueryable<T> query = _data;
    //     if (includes != null)
    //         foreach (var include in includes)
    //             query = query.Include(include);
    //     var obj = query.SingleOrDefault(match);
    //     return obj;
    // }

    // public T? Find(Expression<Func<T, bool>> match)
    // {
    //     var obj = _data.SingleOrDefault(match);
    //     return obj;
    // }


    // public IEnumerable<T> FindAllByName(Expression<Func<T, bool>> match, string[] includes = null)
    // {
    //     IQueryable<T> query = _data;
    //     if (includes != null)
    //         foreach (var include in includes)
    //             query = query.Include(include);
    //     var obj = query.Where(match);
    //     return obj;
    // }
}
