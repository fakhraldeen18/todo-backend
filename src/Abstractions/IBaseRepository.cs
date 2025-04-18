
namespace Harkh_backend.src.Abstractions;

public interface IBaseRepository<T> where T : class
{
    public Task<IEnumerable<T>> FindAll();
    public Task<T?> FindOne(Guid id);
    // public T? Find(Expression<Func<T, bool>> match, string[] includes = null);
    // public IEnumerable<T> FindAllByName(Expression<Func<T, bool>> match, string[] includes = null);
    public Task<T> CreateOne(T newObj);
    public Task<IEnumerable<T>> CreateRange(IEnumerable<T> newObjs);
    public T UpdateOne(T updatedObj);
    public T? DeleteOne(T Obj);
    public IEnumerable<T> DeleteRange(IEnumerable<T> objs);

}
